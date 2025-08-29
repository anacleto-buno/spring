namespace BackendApi.DTOs.Product
{
    /// <summary>
    /// DTO for product response/display
    /// </summary>
    public class ProductResponseDto
    {
        /// <summary>
        /// Product unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Product description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Product category
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Product brand
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Product SKU (Stock Keeping Unit)
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Product release date
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Availability status
        /// </summary>
        public string? AvailabilityStatus { get; set; }

        /// <summary>
        /// Customer rating (0-5)
        /// </summary>
        public decimal? CustomerRating { get; set; }

        /// <summary>
        /// Available colors (comma-separated)
        /// </summary>
        public string? AvailableColors { get; set; }

        /// <summary>
        /// Available sizes (comma-separated)
        /// </summary>
        public string? AvailableSizes { get; set; }

        /// <summary>
        /// When the product was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When the product was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Indicates if the product is in stock
        /// </summary>
        public bool IsInStock => StockQuantity > 0;

        /// <summary>
        /// Formatted price with currency
        /// </summary>
        public string FormattedPrice => Price.ToString("C");

        /// <summary>
        /// List of available colors
        /// </summary>
        public IEnumerable<string> ColorOptions => 
            string.IsNullOrWhiteSpace(AvailableColors) 
                ? Enumerable.Empty<string>() 
                : AvailableColors.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim());

        /// <summary>
        /// List of available sizes
        /// </summary>
        public IEnumerable<string> SizeOptions => 
            string.IsNullOrWhiteSpace(AvailableSizes) 
                ? Enumerable.Empty<string>() 
                : AvailableSizes.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim());
    }
}
