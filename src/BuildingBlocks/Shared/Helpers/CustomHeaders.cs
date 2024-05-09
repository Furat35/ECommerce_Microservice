using System.Text.Json;

namespace Shared.Helpers
{
    public class CustomHeaders
    {
        public Dictionary<string, string> AddPaginationHeader(Metadata metadata)
         => metadata != null
         ? new Dictionary<string, string>() { { "X-Pagination", JsonSerializer.Serialize(metadata) } }
         : throw new Exception();
    }
}
