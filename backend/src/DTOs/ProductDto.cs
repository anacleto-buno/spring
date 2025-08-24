using System.ComponentModel.DataAnnotations;
using BackendApi.Validation;

namespace BackendApi.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string SKU { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public string AvailabilityStatus { get; set; } = string.Empty;
        public decimal? CustomerRating { get; set; }
        public string? AvailableColors { get; set; }
        public string? AvailableSizes { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required")]
        [MaxLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Price must be greater than 0 and less than 999999999999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [SKU]
        public string SKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        [MaxLength(50, ErrorMessage = "Availability status cannot exceed 50 characters")]
        public string AvailabilityStatus { get; set; } = "Available";

        [Range(0, 5, ErrorMessage = "Customer rating must be between 0 and 5")]
        public decimal? CustomerRating { get; set; }

        [MaxLength(500, ErrorMessage = "Available colors cannot exceed 500 characters")]
        public string? AvailableColors { get; set; }

        [MaxLength(500, ErrorMessage = "Available sizes cannot exceed 500 characters")]
        public string? AvailableSizes { get; set; }
    }

    public class UpdateProductDto
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Brand is required")]
        [MaxLength(100, ErrorMessage = "Brand cannot exceed 100 characters")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999999999.99, ErrorMessage = "Price must be greater than 0 and less than 999999999999.99")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be non-negative")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [SKU]
        public string SKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release date is required")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Availability status is required")]
        [MaxLength(50, ErrorMessage = "Availability status cannot exceed 50 characters")]
        public string AvailabilityStatus { get; set; } = "Available";

        [Range(0, 5, ErrorMessage = "Customer rating must be between 0 and 5")]
        public decimal? CustomerRating { get; set; }

        [MaxLength(500, ErrorMessage = "Available colors cannot exceed 500 characters")]
        public string? AvailableColors { get; set; }

        [MaxLength(500, ErrorMessage = "Available sizes cannot exceed 500 characters")]
        public string? AvailableSizes { get; set; }
    }
}
