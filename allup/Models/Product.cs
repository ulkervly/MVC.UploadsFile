using System.ComponentModel.DataAnnotations.Schema;

namespace ALLUP2.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
      
        public double Costprice { get; set; }
        public double Saleprice { get; set; }
        public double DiscountPercent { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        [NotMapped]
        public List<IFormFile>? ImageFiles { get; set; }
        [NotMapped]
        public IFormFile? ProductPosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? ProductHoverImageFile { get; set; }
        [NotMapped]
        public List<int>? ProductImageIds { get; set; }

    }
}
