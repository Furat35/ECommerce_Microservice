using System.Security.Claims;

namespace Shared.Helpers
{
    public static class UserExtensions
    {
        public static string GetActiveUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.Identities.FirstOrDefault()?.Claims.Where(_ => _.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            return userId?.Value;
        }

        public static string GetActiveUserName(this ClaimsPrincipal principal)
        {
            var userId = principal.Identities.FirstOrDefault()?.Claims.Where(_ => _.Type == ClaimTypes.Name).FirstOrDefault();
            return userId?.Value;
        }

        public static string GetActiveUserSurname(this ClaimsPrincipal principal)
        {
            var userId = principal.Identities.FirstOrDefault()?.Claims.Where(_ => _.Type == "Surname").FirstOrDefault();
            return userId?.Value;
        }

        public static string GetActiveUserMail(this ClaimsPrincipal principal)
        {
            var userId = principal.Identities.FirstOrDefault()?.Claims.Where(_ => _.Type == ClaimTypes.Email).FirstOrDefault();
            return userId?.Value;
        }

        public static string GetActiveUserPhone(this ClaimsPrincipal principal)
        {
            var userId = principal.Identities.FirstOrDefault()?.Claims.Where(_ => _.Type == "Phone").FirstOrDefault();
            return userId?.Value;
        }
    }
}
