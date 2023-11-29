namespace ALLUP2.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsPoster { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
