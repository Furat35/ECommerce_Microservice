using Authentication.API.Entities;
using Authentication.API.Enums;
using Authentication.API.Helpers.Common;
using Authentication.API.Models.Dtos.Auth;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using Shared.Exceptions;
using Shared.Helpers.interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authentication.API.Services
{
    public class AuthService(IUserService userService, IPasswordGenerationService passwordGenerationService, ITokenService tokenService,
        ICustomFluentValidationErrorHandling customValidator) : IAuthService
    {
        private readonly IUserService _userService = userService;
        private readonly IPasswordGenerationService _passwordGenerationService = passwordGenerationService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ICustomFluentValidationErrorHandling _customValidator = customValidator;

        public async Task<LoginResponseDto> UserLoginAsync(LoginDto loginUser)
        {
            await _customValidator.ValidateAndThrowAsync(loginUser);
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
            await _customValidator.ValidateAndThrowAsync(registerUser);
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
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, $"{user.Name}"),
                    new("Surname",$"{user.Surname}"),
                    new(ClaimTypes.Email, user.Mail),
                    new("Phone", user.Phone),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, Enum.GetName(user.Role))
            };

            return authClaims;
        }
    }
}
