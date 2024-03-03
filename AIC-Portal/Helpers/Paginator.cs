namespace AIC.Portal.Helpers
{
    public class Paginator<T>
    {
		/// <summary>
		///     The Default Query Parameter Name fot the paginator
		/// </summary>
		public static readonly string ParamName = "page";

		/// <summary>
		///     Constructs a Paginator for Type T with default page size of 10
		/// </summary>
		/// <param name="items">The collection to paginate on</param>
		/// <param name="currentPage">The current page (Default 1)</param>
		/// <param name="pageSize">Number of items to display per page (Default 10)</param>
		/// <returns>
		///     A new Paginator for Type T with items 
		/// </returns>
		public Paginator(IEnumerable<T> items, int currentPage = 1, int pageSize = 10)
        {
            _items = items;
            _pageSize = pageSize;
            _totalPages = (int)Math.Ceiling((double)items.Count() / (double)_pageSize);

            _currentPage = currentPage;
            if (currentPage > _totalPages)
            {
                _currentPage = _totalPages;
            }
            else if (currentPage < 1)
            {
                _currentPage = 1;
            }
        }

        #region Public Properties
        public bool IsFirstPage => _currentPage == 1;
        public bool IsLastPage => _currentPage == _totalPages;
        public bool HasPages => _totalPages > 1;
        public int Pages { get { return _totalPages; } }
        public string NextPageLink { 
            get 
            {
                var pageNumber = IsLastPage ? _totalPages : _currentPage + 1;
                return GetLinkForPage(pageNumber); 
            } 
        }
        public string PreviousPageLink 
        { 
            get 
            {
                var pageNumber = IsFirstPage ? 1 : _currentPage - 1;
                return GetLinkForPage(pageNumber); ; 
            } 
        }
		#endregion
		
        #region Private Properties
		private readonly int _pageSize;
		private readonly int _totalPages;
		private readonly int _currentPage;
		private readonly IEnumerable<T> _items;
		#endregion

		#region Public Methods

		/// <summary>
		///     Paginates on the collection using it's internal state
		/// </summary>
		/// <returns>
		///     A Collection of Items that should be displayed in the current Page
		/// </returns>
		public IEnumerable<T> Paginate()
        {
            return _items.Skip((_currentPage - 1) * _pageSize).Take(_pageSize);
        }

		/// <summary>
		///     HTML Footer Displaying "Prev & Next" links with total Page Numbers
		/// </summary>
		/// <param name="previousText">The Text to be Displayed in the "Previous Page" lin</param>
		/// <param name="nextText">The Text to be Displayed in the "Next Page" link</param>
		/// <returns>
		///     String Representing the Pagination Footer Links HTML
		/// </returns>
		public string Footer(string previousText, string nextText)
		{
			var html = "<ul style='display:flex; gap: 40px; list-style: none;'>";
			if (!IsFirstPage)
			{
				html += $"<li> <a href='{PreviousPageLink}'> {previousText} </a> </li>";
			}
			for (int pageNumber = 1; pageNumber <= Pages; pageNumber++)
			{
				html += IsCurrentPage(pageNumber) ? "<li class='active'>" : "<li>";

				html += $"<a href='{GetLinkForPage(pageNumber)}'>{pageNumber}</a> </li>";
			}
			if (!IsLastPage)
			{
				html += $"<li> <a href='{NextPageLink}'> {nextText} </a> </li>";
			}
			html += "</ul>";

			return html;
		}
        #endregion

        #region Private Methods
        private string GetLinkForPage(int pageNumber)
        {
            return $"?{ParamName}={pageNumber}";
        }
        private bool IsCurrentPage(int pageNumber)
        {
            return _currentPage == pageNumber ;
        }
        #endregion
    }
}
