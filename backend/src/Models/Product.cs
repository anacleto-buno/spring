using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendApi.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = string.Empty;
        
        [Column(TypeName = "date")]
        public DateTime ReleaseDate { get; set; }
        
        [MaxLength(50)]
        public string AvailabilityStatus { get; set; } = "Available";
        
        [Column(TypeName = "decimal(3,2)")]
        public decimal? CustomerRating { get; set; }
        
        [MaxLength(500)]
        public string? AvailableColors { get; set; }
        
        [MaxLength(500)]
        public string? AvailableSizes { get; set; }

        /// <summary>
        /// When the product was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the product was last updated
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
