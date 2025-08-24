using BackendApi.DTOs;
using BackendApi.Models;

namespace BackendApi.Utilities
{
    public static class ProductGenerator
    {
        private static readonly List<string> Categories = new()
        {
            "Electronics", "Clothing", "Home & Garden", "Books", "Sports & Outdoors",
            "Health & Beauty", "Toys & Games", "Automotive", "Food & Beverages",
            "Office Supplies", "Jewelry", "Pet Supplies", "Tools & Hardware"
        };

        private static readonly List<string> Brands = new()
        {
            "TechCorp", "StyleMax", "HomeComfort", "ReadMore", "SportsPro",
            "BeautyPlus", "PlayTime", "AutoParts Inc", "FreshTaste",
            "OfficeSpace", "GlamRock", "PetLove", "BuildIt"
        };

        private static readonly List<string> Colors = new()
        {
            "Red", "Blue", "Green", "Black", "White", "Gray", "Yellow", "Pink",
            "Purple", "Orange", "Brown", "Navy", "Beige", "Silver", "Gold"
        };

        private static readonly List<string> Sizes = new()
        {
            "XS", "S", "M", "L", "XL", "XXL", "One Size", "Small", "Medium", "Large"
        };

        private static readonly List<string> AvailabilityStatuses = new()
        {
            "Available", "Out of Stock", "Limited Stock", "Discontinued", "Pre-order"
        };

        private static readonly Dictionary<string, List<string>> CategoryProducts = new()
        {
            ["Electronics"] = new() { "Smartphone", "Laptop", "Tablet", "Headphones", "Smart Watch", "Camera", "Speaker", "Keyboard", "Mouse", "Monitor" },
            ["Clothing"] = new() { "T-Shirt", "Jeans", "Dress", "Jacket", "Sweater", "Shoes", "Hat", "Scarf", "Gloves", "Socks" },
            ["Home & Garden"] = new() { "Sofa", "Table", "Chair", "Lamp", "Vase", "Plant Pot", "Curtains", "Rug", "Picture Frame", "Candle" },
            ["Books"] = new() { "Novel", "Cookbook", "Biography", "Textbook", "Comic Book", "Poetry", "Self-Help", "History Book", "Science Fiction", "Mystery" },
            ["Sports & Outdoors"] = new() { "Basketball", "Tennis Racket", "Running Shoes", "Yoga Mat", "Bicycle", "Camping Tent", "Backpack", "Water Bottle", "Fitness Tracker", "Dumbbells" },
            ["Health & Beauty"] = new() { "Face Cream", "Shampoo", "Lipstick", "Perfume", "Vitamins", "Moisturizer", "Sunscreen", "Hair Dryer", "Makeup Brush", "Nail Polish" },
            ["Toys & Games"] = new() { "Action Figure", "Board Game", "Puzzle", "Doll", "RC Car", "Building Blocks", "Card Game", "Video Game", "Stuffed Animal", "Art Supplies" },
            ["Automotive"] = new() { "Car Battery", "Oil Filter", "Brake Pads", "Tire", "Car Cover", "Floor Mats", "Air Freshener", "GPS Navigation", "Phone Mount", "Jump Starter" },
            ["Food & Beverages"] = new() { "Organic Coffee", "Green Tea", "Protein Bar", "Olive Oil", "Honey", "Pasta", "Rice", "Spices", "Chocolate", "Wine" },
            ["Office Supplies"] = new() { "Pen", "Notebook", "Stapler", "Paper Clips", "Folder", "Printer Paper", "Desk Organizer", "Calculator", "Scissors", "Tape" },
            ["Jewelry"] = new() { "Necklace", "Ring", "Bracelet", "Earrings", "Watch", "Brooch", "Cufflinks", "Pendant", "Chain", "Anklet" },
            ["Pet Supplies"] = new() { "Dog Food", "Cat Toy", "Pet Bed", "Leash", "Collar", "Fish Tank", "Bird Cage", "Pet Carrier", "Grooming Brush", "Litter Box" },
            ["Tools & Hardware"] = new() { "Hammer", "Screwdriver", "Drill", "Saw", "Wrench", "Pliers", "Level", "Measuring Tape", "Nails", "Screws" }
        };

        public static List<Product> GenerateProducts(int count, ProductDto? template = null)
        {
            var random = new Random();
            var products = new List<Product>();
            var existingSKUs = new HashSet<string>();

            for (int i = 0; i < count; i++)
            {
                // Use template values or generate random ones
                var category = (template != null && !string.IsNullOrEmpty(template.Category))
                    ? template.Category
                    : Categories[random.Next(Categories.Count)];

                var brand = (template != null && !string.IsNullOrEmpty(template.Brand))
                    ? template.Brand
                    : Brands[random.Next(Brands.Count)];

                // Generate product name
                string productName;
                if (template != null && !string.IsNullOrEmpty(template.Name))
                {
                    productName = $"{template.Name} - Variant {i + 1}";
                }
                else
                {
                    var productNames = CategoryProducts.ContainsKey(category)
                        ? CategoryProducts[category]
                        : CategoryProducts["Electronics"];
                    var baseName = productNames[random.Next(productNames.Count)];
                    productName = $"{brand} {baseName}";
                }

                // Generate unique SKU
                string sku;
                if (template != null && !string.IsNullOrEmpty(template.SKU))
                {
                    do
                    {
                        sku = $"{template.SKU}-{i + 1:D4}";
                    } while (existingSKUs.Contains(sku));
                }
                else
                {
                    do
                    {
                        sku = $"{category.Substring(0, Math.Min(3, category.Length)).ToUpper()}-{brand.Substring(0, Math.Min(3, brand.Length)).ToUpper()}-{random.Next(10000, 99999)}";
                    } while (existingSKUs.Contains(sku));
                }
                existingSKUs.Add(sku);

                // Generate price
                decimal price;
                if (template != null && template.Price > 0)
                {
                    // Add some variation to the template price (±20%)
                    var variation = (decimal)(random.NextDouble() * 0.4 - 0.2); // -20% to +20%
                    price = Math.Round(template.Price * (1 + variation), 2);
                    price = Math.Max(0.01m, price); // Ensure minimum price
                }
                else
                {
                    price = Math.Round(5.00m + (decimal)(random.NextDouble() * 995.00), 2);
                }

                // Generate stock quantity
                int stockQuantity;
                if (template != null && template.StockQuantity >= 0)
                {
                    // Add some variation to template stock
                    var variation = random.Next(-50, 51); // ±50 units
                    stockQuantity = Math.Max(0, template.StockQuantity + variation);
                }
                else
                {
                    stockQuantity = random.Next(0, 1000);
                }

                // Generate release date
                DateTime releaseDate;
                if (template != null && template.ReleaseDate != default(DateTime))
                {
                    // Use template date with some random variation (±30 days)
                    var dayVariation = random.Next(-30, 31);
                    releaseDate = template.ReleaseDate.AddDays(dayVariation);
                }
                else
                {
                    releaseDate = DateTime.Now.AddDays(-random.Next(0, 365 * 3));
                }

                // Generate customer rating
                decimal? customerRating;
                if (template != null && template.CustomerRating.HasValue)
                {
                    // Add small variation to template rating (±0.5)
                    var variation = (decimal)(random.NextDouble() * 1.0 - 0.5); // -0.5 to +0.5
                    customerRating = Math.Round(Math.Max(1.0m, Math.Min(5.0m, template.CustomerRating.Value + variation)), 2);
                }
                else
                {
                    customerRating = Math.Round((decimal)(random.NextDouble() * 4 + 1), 2); // 1.00 to 5.00
                }

                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productName,
                    Description = (template != null && !string.IsNullOrEmpty(template.Description))
                        ? $"{template.Description} (Generated variant {i + 1})"
                        : GenerateDescription(productName, brand, random),
                    Category = category,
                    Brand = brand,
                    Price = price,
                    StockQuantity = stockQuantity,
                    SKU = sku,
                    ReleaseDate = releaseDate,
                    AvailabilityStatus = (template != null && !string.IsNullOrEmpty(template.AvailabilityStatus))
                        ? template.AvailabilityStatus
                        : AvailabilityStatuses[random.Next(AvailabilityStatuses.Count)],
                    CustomerRating = customerRating,
                    AvailableColors = (template != null && !string.IsNullOrEmpty(template.AvailableColors))
                        ? template.AvailableColors
                        : string.Join(", ", GetRandomSelection(Colors, random, 1, 4)),
                    AvailableSizes = (template != null && !string.IsNullOrEmpty(template.AvailableSizes))
                        ? template.AvailableSizes
                        : string.Join(", ", GetRandomSelection(Sizes, random, 1, 3))
                };

                products.Add(product);
            }

            return products;
        }

        private static List<string> GetRandomSelection<T>(List<T> source, Random random, int minCount, int maxCount)
        {
            var count = random.Next(minCount, Math.Min(maxCount + 1, source.Count + 1));
            return source.OrderBy(x => random.Next()).Take(count).Select(x => x?.ToString() ?? string.Empty).Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        private static string GenerateDescription(string productName, string brand, Random random)
        {
            var adjectives = new[] { "Premium", "High-quality", "Durable", "Innovative", "Stylish", "Comfortable", "Reliable", "Advanced" };
            var features = new[] { "easy to use", "long-lasting", "versatile", "ergonomic", "eco-friendly", "cutting-edge", "user-friendly", "efficient" };
            
            var adjective = adjectives[random.Next(adjectives.Length)];
            var feature = features[random.Next(features.Length)];
            
            return $"{adjective} {productName} from {brand}. This product is {feature} and designed to meet your needs with exceptional quality and performance.";
        }
    }
}
