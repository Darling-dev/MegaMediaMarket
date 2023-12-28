namespace SiteASPCOm.Models
{
    public class UpdateProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Platform { get; set; }
        public string CardImagePath { get; set; }
        public string BannerImagePath { get; set; }
        public string ScrImagePath { get; set; }
        public string ScrImageAddPath { get; set; }

        // Properties for new file uploads
        public IFormFile NewCardImage { get; set; }
        public IFormFile NewBannerImage { get; set; }
        public IFormFile NewScrImage { get; set; }
        public IFormFile NewScrImageAdd { get; set; }
    }
}