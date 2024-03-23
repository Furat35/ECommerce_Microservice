namespace Shared.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message = "Yetkisiz erişim!") : base(message)
        {

        }
    }
}
