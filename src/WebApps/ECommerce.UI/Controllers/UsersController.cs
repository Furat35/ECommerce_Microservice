using ECommerce.UI.Models.Dtos.Addresses;
using ECommerce.UI.Models.Dtos.PaymentCards;
using ECommerce.UI.Models.Dtos.ViewModels.Users;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using System.Security.Claims;

namespace ECommerce.UI.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public UsersController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserById(HttpContext.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdateUserAddress([FromForm] AddressAddDto address)
        {
            var isUpdated = await _userService.UpdateAddress(address);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.User}")]
        public async Task<IActionResult> UpdateUserPaymentCard([FromForm] PaymentCardAddDto paymentCard)
        {
            var isUpdated = await _userService.UpdatePaymentCard(paymentCard);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> UpdateUserData([FromForm] UserDataUpdateModel userData)
        {
            var isUpdated = await _userService.UpdateData(userData);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> UpdateUserPassword([FromForm] string newPassword)
        {
            var result = await _userService.UpdatePassword(newPassword);
            return RedirectToAction(nameof(AuthController.Logout), "Auth");
        }

        // todo : logout user
        [HttpPost]
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            var result = await _userService.DeleteUser(userId);
            //_authService.Logout()
            return RedirectToAction(nameof(Index));
        }
    }
}
