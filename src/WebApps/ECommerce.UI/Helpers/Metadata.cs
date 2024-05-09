namespace ECommerce.UI.Helpers
{
    public class Metadata
    {

        private int _currentPage;
        private int _pageSize;
        private int _totalPages;
        private int _totalEntities;

        public Metadata()
        {
        }

        public Metadata(int currentPage, int pageSize, int totalEntities, int totalPages)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalEntities = totalEntities;
            TotalPages = totalPages;
        }

        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value > 0 ? value : 0;
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
                _pageSize = value > 0 ? (value > 30 ? 30 : value) : 5;
            }
        }

        public int TotalPages
        {
            get
            {
                return _totalPages;
            }
            set
            {
                _totalPages = value > 0 ? value : 0;
            }
        }

        public int TotalEntities
        {
            get
            {
                return _totalEntities;
            }
            set
            {
                _totalEntities = value > 0 ? value : 0;
            }
        }

        public bool HasNext => TotalPages > CurrentPage;
        public bool HasPrevious => CurrentPage > 1;
        public bool IsValidPage => CurrentPage <= TotalPages;
    }
}
