using System.ComponentModel.DataAnnotations;

namespace BackendApi.DTOs.Product
{
    /// <summary>
    /// DTO for creating a new product
    /// </summary>
    public class ProductCreateDto
    {
        /// <summary>
        /// Product name
        /// </summary>
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 200 characters")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description
        /// </summary>
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        [StringLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string? Category { get; set; }

        /// <summary>
        /// Product brand
        /// </summary>
        [StringLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string? Brand { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99")]
        public decimal Price { get; set; }

        /// <summary>
        /// Stock quantity
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        /// <summary>
        /// Product SKU (Stock Keeping Unit)
        /// </summary>
        [Required(ErrorMessage = "SKU is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "SKU must be between 1 and 100 characters")]
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Product release date
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Availability status
        /// </summary>
        [StringLength(50, ErrorMessage = "Availability status cannot exceed 50 characters")]
        public string? AvailabilityStatus { get; set; }

        /// <summary>
        /// Customer rating (0-5)
        /// </summary>
        [Range(0.0, 5.0, ErrorMessage = "Customer rating must be between 0 and 5")]
        public decimal? CustomerRating { get; set; }

        /// <summary>
        /// Available colors (comma-separated)
        /// </summary>
        [StringLength(500, ErrorMessage = "Available colors cannot exceed 500 characters")]
        public string? AvailableColors { get; set; }

        /// <summary>
        /// Available sizes (comma-separated)
        /// </summary>
        [StringLength(500, ErrorMessage = "Available sizes cannot exceed 500 characters")]
        public string? AvailableSizes { get; set; }
    }
}
