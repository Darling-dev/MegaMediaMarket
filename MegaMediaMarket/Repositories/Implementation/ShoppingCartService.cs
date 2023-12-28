using SiteASPCOm.Models;
using SiteASPCOm.Models.Domain;
using SiteASPCOm.Models.DTO;
using SiteASPCOm.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

namespace SiteASPCOm.Repositories.Implementation
{
    public class ShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddItem(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(p => p.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new ShoppingCartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    Link = product.Link,
                });
            }
            SaveCart(cart);
        }

        private List<ShoppingCartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString(CartSessionKey);
            return string.IsNullOrEmpty(cartJson) ? new List<ShoppingCartItem>() : JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cartJson);
        }

        private void SaveCart(List<ShoppingCartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }
        public List<ShoppingCartItem> GetItems()
        {
            return GetCart();
        }
        public void RemoveItem(Guid productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        // ... other methods ...
    }

}
