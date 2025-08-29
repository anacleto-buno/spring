namespace BackendApi.DTOs.Common
{
    /// <summary>
    /// Represents a paged result with metadata
    /// </summary>
    /// <typeparam name="T">The type of items in the page</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// The items in the current page
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// Indicates if there is a previous page
        /// </summary>
        public bool HasPrevious => Page > 1;

        /// <summary>
        /// Indicates if there is a next page
        /// </summary>
        public bool HasNext => Page < TotalPages;

        /// <summary>
        /// First item number on current page
        /// </summary>
        public int FirstItemOnPage => ((Page - 1) * PageSize) + 1;

        /// <summary>
        /// Last item number on current page
        /// </summary>
        public int LastItemOnPage => Math.Min(Page * PageSize, TotalCount);

        /// <summary>
        /// Creates a new paged result
        /// </summary>
        /// <param name="items">The items for the current page</param>
        /// <param name="totalCount">Total number of items</param>
        /// <param name="page">Current page number</param>
        /// <param name="pageSize">Number of items per page</param>
        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// Creates an empty paged result
        /// </summary>
        public PagedResult()
        {
        }
    }
}
