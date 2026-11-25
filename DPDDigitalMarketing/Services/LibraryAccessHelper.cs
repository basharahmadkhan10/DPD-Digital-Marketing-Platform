using System.Text.Json;
using DPDDigitalMarketing.Data;

namespace DPDDigitalMarketing.Services
{

    public static class LibraryAccessHelper
    {
        private const string UnlockedSessionKey = "DPD_Unlocked_Products";

        public static List<int> GetUnlockedProductIds(ISession session)
        {
            var json = session.GetString(UnlockedSessionKey);
            if (string.IsNullOrEmpty(json)) return new List<int>();
            try
            {
                return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
            }
            catch
            {
                return new List<int>();
            }
        }

        public static void UnlockProducts(ISession session, IEnumerable<int> productIds)
        {
            var current = GetUnlockedProductIds(session);
            foreach (var id in productIds)
            {
                if (!current.Contains(id))
                {
                    current.Add(id);
                }
            }
            session.SetString(UnlockedSessionKey, JsonSerializer.Serialize(current));
        }

        public static bool IsProductUnlocked(ISession session, int productId, string? userEmail, AppDbContext context)
        {

            var sessionIds = GetUnlockedProductIds(session);
            if (sessionIds.Contains(productId)) return true;

            if (!string.IsNullOrEmpty(userEmail))
            {
                var hasPaidInDb = context.Orders
                    .Where(o => o.Status == "Completed" && o.CustomerEmail.ToLower() == userEmail.ToLower())
                    .Any(o => o.OrderItems.Any(oi => oi.ProductId == productId));

                if (hasPaidInDb) return true;
            }

            return false;
        }
    }
}
