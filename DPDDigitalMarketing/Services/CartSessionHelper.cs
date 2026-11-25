using System.Text.Json;
using DPDDigitalMarketing.Models;

namespace DPDDigitalMarketing.Services
{

    public static class CartSessionHelper
    {
        private const string CartSessionKey = "DPD_User_Cart";

        public static List<CartItem> GetCart(ISession session)
        {
            var json = session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(json))
            {
                return new List<CartItem>();
            }
            try
            {
                return JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
            }
            catch
            {
                return new List<CartItem>();
            }
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            session.SetString(CartSessionKey, json);
        }

        public static int GetTotalCount(ISession session)
        {
            var cart = GetCart(session);
            return cart.Sum(i => i.Quantity);
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartSessionKey);
        }
    }
}
