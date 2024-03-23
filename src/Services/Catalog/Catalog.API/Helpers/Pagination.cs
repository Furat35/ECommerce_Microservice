namespace Catalog.API.Helpers
{
    public class Pagination
    {
        int _page = 0;
        int _pageSize = 10;

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value < 0 ? 0 : value;
            }
        }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value <= 0 ? 10 : (value > 100 ? 100 : value);
            }
        }
    }
}
