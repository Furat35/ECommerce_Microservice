using ECommerce.UI.Models.Dtos.Auth;
using ECommerce.UI.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            returnUrl ??= Url.Content("/Products/GetProducts"); // Default return URL if not provided
            TempData["ReturnUrl"] = returnUrl;

            return User.Identity.IsAuthenticated
                ? RedirectToAction(nameof(ProductsController.GetProducts), "Products")
                : View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var loginResult = await _authService.LoginAsync(loginDto);
            return loginResult
             ? LocalRedirect(TempData["ReturnUrl"].ToString())
             : View();
            //return loginResult
            //     ? RedirectToAction(nameof(ProductsController.GetProducts), "Products")
            //     : View();
        }

        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(ProductsController.GetProducts), "Products");
            }
            await _authService.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(ProductsController.GetProducts), "Products");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(ProductsController.GetProducts), "Products");
            }
            bool isRegistered = await _authService.RegisterAsync(registerDto);
            return isRegistered ? RedirectToAction(nameof(Login)) : View();
        }
    }
}
