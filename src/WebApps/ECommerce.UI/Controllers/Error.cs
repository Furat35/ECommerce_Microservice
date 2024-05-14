using ECommerce.UI.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerce.UI.Controllers
{
    public class Error : Controller
    {
        public IActionResult Index()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            HttpStatusCode? statusCode = (HttpStatusCode)exception?.GetType()?.GetProperty("StatusCode")?.GetValue(exception);
            ErrorDetail errorDetails = new()
            {
                ErrorMessage = exception.Message,
                StatusCode = statusCode != null ? (int)statusCode : 0
            };

            return View(errorDetails);
        }
    }
}
