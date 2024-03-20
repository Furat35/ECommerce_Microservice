using Authentication.API.Entities;
using Authentication.API.Enums;
using Authentication.API.Exceptions;
using Authentication.API.Helpers.Common;
using Authentication.API.Models.Dtos.Auth;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using FluentValidation;
using FluentValidation.Results;
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

        public async Task<string> UserLoginAsync(LoginDto loginUser)
        {
            var validationResult = await _loginDtoValidator.ValidateAsync(loginUser);
            ThrowBadRequestIfDtoNotValid(validationResult);
            var user = await _userService.GetUserByMailAsync(loginUser.Mail);
            bool isValid = _passwordGenerationService.VerifyPassword(user.PasswordSalt, user.Password, loginUser.Password);
            if (isValid)
            {
                var claims = ConfigureUserClaims(user);
                var token = _tokenService.GenerateToken(claims);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            throw new BadRequestException("Hatalı mail adresi veya şifre!");

        }

        public async Task<bool> UserRegisterAsync(RegisterDto registerUser, Role role)
        {
            var validationResult = await _registerDtoValidator.ValidateAsync(registerUser);
            ThrowBadRequestIfDtoNotValid(validationResult);
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
                    new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                    new Claim(ClaimTypes.Email, user.Mail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, Enum.GetName(user.Role))
            };

            return authClaims;
        }

        private void ThrowBadRequestIfDtoNotValid(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
        }
    }
}
