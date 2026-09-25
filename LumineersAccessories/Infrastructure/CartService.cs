using LumineersAccessories.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LumineersAccessories.Infrastructure
{
    /// <summary>
    /// Session-backed shopping cart. The cart is stored as JSON in the user's session
    /// and is only persisted to the Orders table at checkout.
    /// </summary>
    public class CartService
    {
        public const string SessionKey = "Cart";

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            var json = session?.GetString(SessionKey);
            return json == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetString(SessionKey, JsonConvert.SerializeObject(cart));
        }

        public void Add(int productId, string productName, decimal price, int quantity, string? imageUrl, int stockLevel)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == productId);
            if (item != null)
            {
                item.Quantity = Math.Min(item.Quantity + quantity, stockLevel);
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductID = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = Math.Min(quantity, stockLevel),
                    ImageUrl = imageUrl,
                    StockLevel = stockLevel
                });
            }
            SaveCart(cart);
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductID == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    cart.Remove(item);
                }
                else
                {
                    item.Quantity = Math.Min(quantity, item.StockLevel);
                }
                SaveCart(cart);
            }
        }

        public void Remove(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(c => c.ProductID == productId);
            SaveCart(cart);
        }

        public void Clear()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Remove(SessionKey);
        }
    }
}
