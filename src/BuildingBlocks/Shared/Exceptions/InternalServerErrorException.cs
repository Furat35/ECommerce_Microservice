namespace Shared.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message = "İşlem sırasında beklenmeyen bir hata oluştu!") : base(message)
        {

        }
    }
}
