using Authentication.API.DataAccess.Contexts;
using Authentication.API.DataAccess.Repositories;
using Authentication.API.Entities;
using Authentication.API.Exceptions;
using Authentication.API.Helpers;
using Authentication.API.Helpers.Common;
using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;
using Authentication.API.Models.Dtos.User;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using Authentication.API.Validations.PaymentCards;
using AutoMapper;
using FluentValidation;
using System.Text;

namespace Authentication.API.Services
{
    public class UserService : RepositoryBase<User>, IUserService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordGenerationService _passwordGenerationService;
        private readonly string _activeUserId;
        private readonly IValidator<UserAddDto> _userAddDtoValidator;
        private readonly IValidator<AddressAddDto> _addressAddDtoValidator;
        private readonly IValidator<PaymentCardAddDto> _paymentCardAddDtoValidator;

        public UserService(AuthenticationContext context, IMapper mapper, IPasswordGenerationService passwordGenerationService,
            IHttpContextAccessor httpContextAccessor, IValidator<UserAddDto> userAddDtoValidator, IValidator<AddressAddDto> addressAddDtoValidator, 
            IValidator<PaymentCardAddDto> paymentCardAddDtoValidator) : base(context)
        {
            _mapper = mapper;
            _passwordGenerationService = passwordGenerationService;
            _activeUserId = httpContextAccessor.HttpContext.User.GetActiveUserId();
            _userAddDtoValidator = userAddDtoValidator;
            _addressAddDtoValidator = addressAddDtoValidator;
            _paymentCardAddDtoValidator = paymentCardAddDtoValidator;
        }

        public async Task<bool> AddUserAsync(UserAddDto user)
        {
            await UserAddDtoValidator(user);
            var userToAdd = _mapper.Map<User>(user);

            var generatePassword = GeneratePasswordHash(user.Password);
            userToAdd.Password = generatePassword.storedHashedPassword;
            userToAdd.PasswordSalt = generatePassword.storedSalt;

            var result = await AddAsync(userToAdd);

            return result != null;
        }

        public async Task<bool> CheckIfUserExists(string email)
        {
            var user = (await GetAsync(_ => _.Mail.ToUpper() == email.ToUpper())).FirstOrDefault();
            return user != null;
        }

        public async Task<UserListDto> GetUserByIdAsync(string userId)
        {
            var user = await GetByIdAsync(Guid.Parse(userId), [_ => _.PaymentCard, _ => _.Address]);
            return _mapper.Map<UserListDto>(user);
        }

        public async Task<bool> UpdateAddress(AddressAddDto address)
        {
            await ValidateAddressAddDto(address);
            var user = (await GetAsync(_ => _.Id == Guid.Parse(_activeUserId), [_ => _.Address])).FirstOrDefault();
            if (user.Address is null)
                user.Address = new Address();
            _mapper.Map(address, user.Address);
            var effectedRows = await UpdateAsync(user);

            return effectedRows != 0;
        }

        public async Task<bool> UpdatePaymentCard(PaymentCardAddDto paymentCard)
        {
            await ValidatePaymentCardAddDto(paymentCard);
            var user = (await GetAsync(_ => _.Id == Guid.Parse(_activeUserId), [_ => _.PaymentCard])).FirstOrDefault();
            if (user.PaymentCard is null)
                user.PaymentCard = new PaymentCard();
            _mapper.Map(paymentCard, user.PaymentCard);
            var effectedRows = await UpdateAsync(user);

            return effectedRows != 0;
        }

        public async Task<User> GetUserByMailAsync(string mail)
        {
            var user = (await GetAsync(_ => _.Mail.ToLower() == mail.ToLower())).FirstOrDefault();
            return user;
        }

        public async Task<bool> SafeDeleteUserAsync(string userId)
        {
            var user = await GetByIdAsync(Guid.Parse(userId));
            if (user is null)
                return false;

            user.IsDeleted = true;
            var result = await UpdateAsync(user);
            return result != null;
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
                await ValidateAddressAddDto(user.Address);
            if (user.PaymentCard != null)
                await ValidatePaymentCardAddDto(user.PaymentCard);
        }

        private async Task ValidateAddressAddDto(AddressAddDto address)
        {
            var addressState = await _addressAddDtoValidator.ValidateAsync(address);
            if (!addressState.IsValid)
                throw new BadRequestException(addressState.Errors.First().ErrorMessage);
        }

        private async Task ValidatePaymentCardAddDto(PaymentCardAddDto paymentCard)
        {
            var validationResult = await _paymentCardAddDtoValidator.ValidateAsync(paymentCard);
            if (!validationResult.IsValid)
                throw new BadRequestException(validationResult.Errors.First().ErrorMessage);
        }
    }
}
