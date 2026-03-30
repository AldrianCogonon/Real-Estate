namespace capstones.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public string? Frequency { get; set; }
        public string? Overview { get; set; }
        public string? AgentName { get; set; }
        public string? SellerName { get; set; }
        public string Category { get; set; } = "";
        public string Address { get; set; } = "";
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string? MapLink { get; set; }
        public List<string> Amenities { get; set; } = new();
        public List<string> Images { get; set; } = new();
    }

    public class ProductListItemDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public string? Frequency { get; set; }
        public string Category { get; set; } = "";
        public string Address { get; set; } = "";
        public string ThumbnailUrl { get; set; } = "";
    }

    public class CreateProductRequest
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = "";
        public decimal Price { get; set; }
        public string? Frequency { get; set; }
        public string? Overview { get; set; }
        public string? AgentName { get; set; }
        public string? SellerName { get; set; }
        public string Category { get; set; } = "";
        public string Address { get; set; } = "";
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string? MapLink { get; set; }
        public List<string> Amenities { get; set; } = new();
        public List<string> Images { get; set; } = new();
    }
}