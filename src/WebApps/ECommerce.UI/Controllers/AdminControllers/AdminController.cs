using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.UI.Controllers.AdminControllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public AdminController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userService.GetUserById(HttpContext.User.FindFirst(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            return View(user);
        }
    }
}
