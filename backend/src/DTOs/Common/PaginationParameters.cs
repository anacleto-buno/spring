using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs.Common
{
    /// <summary>
    /// Base pagination parameters
    /// </summary>
    public class PaginationParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        /// <summary>
        /// Page number (1-based)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page
        /// </summary>
        [Range(1, MaxPageSize, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        /// <summary>
        /// Sort field
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// Sort direction (asc/desc)
        /// </summary>
        public string SortDirection { get; set; } = "asc";

        /// <summary>
        /// Indicates if sort direction is descending
        /// </summary>
        public bool IsDescending => SortDirection?.ToLower() == "desc";
    }
}
