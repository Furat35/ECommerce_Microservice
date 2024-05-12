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
            HttpStatusCode statusCode = (HttpStatusCode)exception.GetType().GetProperty("StatusCode").GetValue(exception);
            object test;
            var val = (int)statusCode;
            ErrorDetail errorDetails = new()
            {
                ErrorMessage = exception.Message,
                StatusCode = (int)statusCode
            };

            return View(errorDetails);
        }
    }
}
