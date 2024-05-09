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
using FluentValidation;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.Helpers;
using System.Text;

namespace Authentication.API.Services
{
    public class UserService : RepositoryBase<User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly string _activeUserId;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;
        private readonly IValidator<AddressAddDto> _addressAddDtoValidator;
        private readonly IValidator<PaymentCardAddDto> _paymentCardAddDtoValidator;

        public UserService(AuthenticationContext context, IMapper mapper, IPasswordGenerationService passwordGenerationService,
            IHttpContextAccessor httpContextAccessor, IValidator<UserAddDto> userAddDtoValidator, IValidator<AddressAddDto> addressAddDtoValidator,
            IValidator<PaymentCardAddDto> paymentCardAddDtoValidator, IValidator<UserUpdateDto> userUpdateDtoValidator) : base(context)
        {
            _mapper = mapper;
            _passwordGenerationService = passwordGenerationService;
            _activeUserId = httpContextAccessor.HttpContext.User.GetActiveUserId(); // todo : throws exception??
            _userAddDtoValidator = userAddDtoValidator;
            _addressAddDtoValidator = addressAddDtoValidator;
            _paymentCardAddDtoValidator = paymentCardAddDtoValidator;
            _userUpdateDtoValidator = userUpdateDtoValidator;
        }

        public async Task<bool> AddUserAsync(UserAddDto user)
        {
            await UserAddDtoValidator(user);
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
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(address, _addressAddDtoValidator);
            var user = await GetByIdAsync(Guid.Parse(_activeUserId), [_ => _.Address]);
            if (user.Address is null)
                user.Address = new Address();
            _mapper.Map(address, user.Address);

            return await UpdateAsync(user) != 0;
        }

        public async Task<bool> UpdatePaymentCardAsync(PaymentCardAddDto paymentCard)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(paymentCard, _paymentCardAddDtoValidator);
            var user = (await GetAsync(_ => _.Id == Guid.Parse(_activeUserId), [_ => _.PaymentCard])).FirstOrDefault();
            if (user.PaymentCard is null)
                user.PaymentCard = new PaymentCard();
            _mapper.Map(paymentCard, user.PaymentCard);

            return await UpdateAsync(user) != 0;
        }

        public async Task<bool> UpdateDataAsync(UserUpdateDto userDto, string userId)
        {
            await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(userDto, _userUpdateDtoValidator);
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

        private async Task UserAddDtoValidator(UserAddDto user)
        {
            var userState = await _userAddDtoValidator.ValidateAsync(user);
            if (!userState.IsValid)
                throw new BadRequestException(userState.Errors.First().ErrorMessage);
            if (user.Address != null)
                await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(user.Address, _addressAddDtoValidator);
            if (user.PaymentCard != null)
                await CustomFluentValidationErrorHandling.ValidateAndThrowAsync(user.PaymentCard, _paymentCardAddDtoValidator);
        }
    }
}
