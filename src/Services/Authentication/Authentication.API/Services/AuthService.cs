using Authentication.API.Entities;
using Authentication.API.Enums;
using Authentication.API.Helpers.Common;
using Authentication.API.Models.Dtos.Auth;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using FluentValidation;
using Shared.Exceptions;
using Shared.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly ITokenService _tokenService;
        private readonly IValidator<RegisterDto> _registerDtoValidator;
        private readonly IValidator<LoginDto> _loginDtoValidator;

        public AuthService(IUserService userService, IPasswordGenerationService passwordGenerationService, ITokenService tokenService,
            IValidator<RegisterDto> registerDtoValidator, IValidator<LoginDto> loginDtoValidator)
        {
            _userService = userService;
            _passwordGenerationService = passwordGenerationService;
            _tokenService = tokenService;
            _registerDtoValidator = registerDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }

        public async Task<LoginResponseDto> UserLoginAsync(LoginDto loginUser)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(loginUser, _loginDtoValidator);
            var user = await _userService.GetUserByMailAsync(loginUser.Mail);
            if (user is null)
                throw new BadRequestException("Hatalı mail adresi veya şifre!");
            bool isValid = _passwordGenerationService.VerifyPassword(user.PasswordSalt, user.Password, loginUser.Password);
            if (isValid)
            {
                var claims = ConfigureUserClaims(user);
                var token = _tokenService.GenerateToken(claims);

                return new LoginResponseDto
                {
                    Id = user.Id.ToString(),
                    Mail = user.Mail,
                    Name = user.Name,
                    Surname = user.Surname,
                    Phone = user.Phone,
                    Role = Enum.GetName(user.Role),
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }

            throw new BadRequestException("Hatalı mail adresi veya şifre!");
        }

        public async Task<bool> UserRegisterAsync(RegisterDto registerUser, Role role)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(registerUser, _registerDtoValidator);
            var user = new UserAddDto
            {
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Mail = registerUser.Mail,
                Password = registerUser.Password,
                Phone = registerUser.Phone,
                Role = role
            };

            var isCreated = await _userService.AddUserAsync(user);
            return isCreated;
        }

        private List<Claim> ConfigureUserClaims(User user)
        {
            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.Name}"),
                    new Claim("Surname",$"{user.Surname}"),
                    new Claim(ClaimTypes.Email, user.Mail),
                    new Claim("Phone", user.Phone),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, Enum.GetName(user.Role))
            };

            return authClaims;
        }
    }
}
