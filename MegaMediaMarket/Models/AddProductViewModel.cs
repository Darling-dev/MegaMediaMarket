using System.ComponentModel.DataAnnotations.Schema;

namespace SiteASPCOm.Models
{
    public class AddProductViewModel
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Platform { get; set; }
        [NotMapped]
        public IFormFile CardImage { get; set; }
        [NotMapped]
        public IFormFile BannerImage { get; set; }
        [NotMapped]
        public IFormFile ScrImage { get; set; }
        [NotMapped]
        public IFormFile ScrImageAdd { get; set; }
    }
}