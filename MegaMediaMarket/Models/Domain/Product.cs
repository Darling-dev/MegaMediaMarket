using System.ComponentModel.DataAnnotations.Schema;

namespace SiteASPCOm.Models.Domain
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string Platform { get; set; }
        [NotMapped]
        public IFormFile CardImage { get; set; }
        [NotMapped]
        public IFormFile BannerImage { get; set; }
        [NotMapped]
        public IFormFile ScrImage { get; set; }
        [NotMapped]
        public IFormFile ScrImageAdd { get; set; }
        public string CardImagePath { get; set; }
        public string BannerImagePath { get; set; }
        public string ScrImagePath { get; set; }
        public string ScrImageAddPath { get; set; }
    }
}