namespace Shared.Helpers
{
    public class Pagination
    {
        int _page = 1;
        int _pageSize = 20;

        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value < 0 ? 1 : value;
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
                _pageSize = value <= 0 ? 20 : (value > 100 ? 100 : value);
            }
        }
    }
}
