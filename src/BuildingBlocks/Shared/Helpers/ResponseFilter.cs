namespace Shared.Helpers
{
    public class ResponseFilter<T> where T : class, new()
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public T ResponseValue { get; set; } = new T();
    }
}
