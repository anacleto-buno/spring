namespace BackendApi.DTOs.Product
{
    /// <summary>
    /// DTO for product list display with minimal information
    /// </summary>
    public class ProductListDto
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
        /// Product SKU (Stock Keeping Unit)
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Availability status
        /// </summary>
        public string? AvailabilityStatus { get; set; }

        /// <summary>
        /// Customer rating (0-5)
        /// </summary>
        public decimal? CustomerRating { get; set; }

        /// <summary>
        /// Stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Indicates if the product is in stock
        /// </summary>
        public bool IsInStock => StockQuantity > 0;

        /// <summary>
        /// Formatted price with currency
        /// </summary>
        public string FormattedPrice => Price.ToString("C");
    }
}
