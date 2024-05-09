using Microsoft.AspNetCore.Http;

namespace Shared.Helpers
{
    public class HeaderService
    {
        private readonly IHttpContextAccessor _httpContext;

        public HeaderService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public void AddToHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
                _httpContext.HttpContext?.Response.Headers.Add(header.Key, header.Value);
        }
    }
}
