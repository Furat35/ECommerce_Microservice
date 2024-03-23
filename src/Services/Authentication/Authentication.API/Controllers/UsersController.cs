using Authentication.API.Models.Dtos.Addresses;
using Authentication.API.Models.Dtos.PaymentCards;
using Authentication.API.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPut("address")]
        public async Task<IActionResult> UpdateUserAddress([FromBody] AddressAddDto address)
        {
            var isUpdated = await _userService.UpdateAddress(address);
            return Ok(isUpdated);
        }

        [HttpPut("paymentCard")]
        public async Task<IActionResult> UpdateUserPaymentCard([FromBody] PaymentCardAddDto paymentCard)
        {
            var isUpdated = await _userService.UpdatePaymentCard(paymentCard);
            return Ok(isUpdated);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserPassword([FromBody] string password)
        {
            var httpContext = HttpContext;
            var result = await _userService.UpdateUserPasswordAsync(HttpContext.User.GetActiveUserId(), password);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUsers(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }


        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userService.SafeDeleteUserAsync(userId);
            return Ok(user);
        }
    }
}
