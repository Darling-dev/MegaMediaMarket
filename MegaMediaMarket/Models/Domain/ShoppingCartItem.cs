namespace SiteASPCOm.Models.Domain
{
    public class ShoppingCartItem
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public string Link { get; set; }
    }
}
