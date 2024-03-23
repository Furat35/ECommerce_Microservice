namespace Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Bulunamadı!") : base(message)
        {

        }
    }
}
