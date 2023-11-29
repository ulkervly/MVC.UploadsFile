using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PustokPractice.Models
{
    public class Slider:BaseEntity
    {
        
        [Required]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Description { get; set; }
        public string? Image {  get; set; }
        [NotMapped]
        public IFormFile? ImageFile {  get; set; }
        public string RedirectUrl {  get; set; }
        public string ButtonText { get; set; }
    }
}
