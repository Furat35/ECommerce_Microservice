using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;
using Authentication.API.Models.Dtos.Users;
using Authentication.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.Exceptions;
using Shared.Helpers;

namespace Authentication.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("data")]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdateUserData([FromBody] UserUpdateDto user)
        {
            var isUpdated = await _userService.UpdateDataAsync(user, HttpContext.User.GetActiveUserId());
            return Ok(isUpdated);
        }

        [HttpPut("address")]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressAddDto address)
        {
            var isUpdated = await _userService.UpdateAddressAsync(address);
            return Ok(isUpdated);
        }

        [HttpPut("paymentCard")]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdatePaymentCard([FromBody] PaymentCardAddDto paymentCard)
        {
            var isUpdated = await _userService.UpdatePaymentCardAsync(paymentCard);
            return Ok(isUpdated);
        }

        [HttpPut]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdatePassword([FromBody] string password)
        {
            var result = await _userService.UpdateUserPasswordAsync(HttpContext.User.GetActiveUserId(), password);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUsers(string userId)
        {
            if (!(User.IsInRole(Role.Admin) || User.GetActiveUserId().Equals(userId, StringComparison.InvariantCultureIgnoreCase)))
                throw new ForbiddenException();

            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (!(User.IsInRole(Role.Admin) || User.GetActiveUserId().Equals(userId, StringComparison.InvariantCultureIgnoreCase)))
                throw new ForbiddenException();

            var user = await _userService.SafeDeleteUserAsync(userId);
            return Ok(user);
        }
    }
}
