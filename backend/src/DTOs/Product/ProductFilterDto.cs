using BackendApi.DTOs.Common;
using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs.Product
{
    /// <summary>
    /// DTO for filtering and searching products
    /// </summary>
    public class ProductFilterDto : PaginationParameters
    {
        /// <summary>
        /// Search term for name, description, category, brand, or SKU
        /// </summary>
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Filter by category
        /// </summary>
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        /// <summary>
        /// Filter by brand
        /// </summary>
        [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string? Brand { get; set; }

        /// <summary>
        /// Minimum price filter
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Minimum price cannot be negative")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Maximum price filter
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Maximum price cannot be negative")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Filter by availability status
        /// </summary>
        [StringLength(50, ErrorMessage = "Availability status cannot exceed 50 characters")]
        public string? AvailabilityStatus { get; set; }

        /// <summary>
        /// Minimum rating filter
        /// </summary>
        [Range(0.0, 5.0, ErrorMessage = "Minimum rating must be between 0 and 5")]
        public decimal? MinRating { get; set; }

        /// <summary>
        /// Filter for in-stock products only
        /// </summary>
        public bool? InStockOnly { get; set; }

        /// <summary>
        /// Filter by available colors
        /// </summary>
        [StringLength(100, ErrorMessage = "Color filter cannot exceed 100 characters")]
        public string? Color { get; set; }

        /// <summary>
        /// Filter by available sizes
        /// </summary>
        [StringLength(100, ErrorMessage = "Size filter cannot exceed 100 characters")]
        public string? Size { get; set; }

        /// <summary>
        /// Filter by release date range - from
        /// </summary>
        public DateTime? ReleaseDateFrom { get; set; }

        /// <summary>
        /// Filter by release date range - to
        /// </summary>
        public DateTime? ReleaseDateTo { get; set; }

        /// <summary>
        /// Validates the filter parameters
        /// </summary>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValid()
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
                return false;

            if (ReleaseDateFrom.HasValue && ReleaseDateTo.HasValue && ReleaseDateFrom > ReleaseDateTo)
                return false;

            return true;
        }

        /// <summary>
        /// Gets validation error message if invalid
        /// </summary>
        /// <returns>Error message or null if valid</returns>
        public string? GetValidationError()
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
                return "Minimum price cannot be greater than maximum price";

            if (ReleaseDateFrom.HasValue && ReleaseDateTo.HasValue && ReleaseDateFrom > ReleaseDateTo)
                return "Release date from cannot be greater than release date to";

            return null;
        }
    }
}
