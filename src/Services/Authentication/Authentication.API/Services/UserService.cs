using Authentication.API.DataAccess.Contexts;
using Authentication.API.DataAccess.Repositories;
using Authentication.API.Entities;
using Authentication.API.Helpers.Common;
using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;
using Authentication.API.Models.Dtos.User;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using AutoMapper;
using Shared.Exceptions;
using Shared.Helpers;
using Shared.Helpers.interfaces;
using System.Text;

namespace Authentication.API.Services
{
    public class UserService(AuthenticationContext context, IMapper mapper, IPasswordGenerationService passwordGenerationService,
        IHttpContextAccessor httpContextAccessor, ICustomFluentValidationErrorHandling customValidator) : RepositoryBase<User>(context), IUserService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordGenerationService _passwordGenerationService = passwordGenerationService;
        private readonly ICustomFluentValidationErrorHandling _customValidator = customValidator;
        private readonly string _activeUserId = httpContextAccessor.HttpContext.User.GetActiveUserId();

        public async Task<bool> AddUserAsync(UserAddDto user)
        {
            await _customValidator.ValidateAndThrowAsync(user);
            var userToAdd = _mapper.Map<User>(user);

            var generatePassword = GeneratePasswordHash(user.Password);
            userToAdd.Password = generatePassword.storedHashedPassword;
            userToAdd.PasswordSalt = generatePassword.storedSalt;

            var result = await AddAsync(userToAdd);

            return result != 0;
        }

        public async Task<UserListDto> GetUserByIdAsync(string userId)
        {
            var user = await GetByIdAsync(Guid.Parse(userId), [_ => _.PaymentCard, _ => _.Address]);
            if (user is null)
                throw new NotFoundException("Kullanıcı bulunamadı!");

            return _mapper.Map<UserListDto>(user);
        }

        public async Task<bool> UpdateAddressAsync(AddressAddDto address)
        {
            await _customValidator.ValidateAndThrowAsync(address);
            var user = await GetByIdAsync(Guid.Parse(_activeUserId), [_ => _.Address]);
            if (user.Address is null)
                user.Address = new Address();
            _mapper.Map(address, user.Address);

            return await UpdateAsync(user) != 0;
        }

        public async Task<bool> UpdatePaymentCardAsync(PaymentCardAddDto paymentCard)
        {
            await _customValidator.ValidateAndThrowAsync(paymentCard);
            var user = (await GetAsync(_ => _.Id == Guid.Parse(_activeUserId), [_ => _.PaymentCard])).FirstOrDefault();
            if (user.PaymentCard is null)
                user.PaymentCard = new PaymentCard();
            _mapper.Map(paymentCard, user.PaymentCard);

            return await UpdateAsync(user) != 0;
        }

        public async Task<bool> UpdateDataAsync(UserUpdateDto userDto, string userId)
        {
            await _customValidator.ValidateAndThrowAsync(userDto);
            var user = await GetByIdAsync(Guid.Parse(userId));
            if (user is null)
                throw new NotFoundException("Kullanıcı bulunamadı!");

            _mapper.Map(userDto, user);

            return await UpdateAsync(user) != 0;

        }

        public async Task<bool> UpdateUserPasswordAsync(string userId, string password)
        {
            var user = await GetByIdAsync(Guid.Parse(userId), [_ => _.PaymentCard, _ => _.Address]);
            if (user is null)
                return false;

            var generatePassword = GeneratePasswordHash(password);
            user.Password = generatePassword.storedHashedPassword;
            user.PasswordSalt = generatePassword.storedSalt;

            return await UpdateAsync(user) != 0;
        }

        public async Task<User> GetUserByMailAsync(string mail)
            => (await GetAsync(_ => _.Mail.ToLower() == mail.ToLower())).FirstOrDefault();

        public async Task<bool> CheckIfUserExistsAsync(string email)
            => (await GetAsync(_ => _.Mail.ToUpper() == email.ToUpper())).FirstOrDefault() != null;

        public async Task<bool> SafeDeleteUserAsync(string userId)
        {
            var user = await GetByIdAsync(Guid.Parse(userId));
            if (user is null)
                return false;

            user.IsDeleted = true;
            return await UpdateAsync(user) != 0;
        }

        private (string storedSalt, string storedHashedPassword) GeneratePasswordHash(string password)
        {
            var salt = _passwordGenerationService.GenerateSalt();
            var combinedBytes = _passwordGenerationService.Combine(Encoding.UTF8.GetBytes(password), salt);
            var hashedBytes = _passwordGenerationService.HashBytes(combinedBytes);
            string storedSalt = Convert.ToBase64String(salt);
            string storedHashedPassword = Convert.ToBase64String(hashedBytes);

            return (storedSalt, storedHashedPassword);
        }
    }
}
