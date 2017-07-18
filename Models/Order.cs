using System.Collections.Generic;

namespace KofileDotNet.Models
{
    public class OrderItem
    {
        public int order_item_id { get; set; }
        public string type { get; set; }
        public int pages { get; set; }
    }

    public class Order
    {
        public string order_date { get; set; }
        public string order_number { get; set; }
        public List<OrderItem> order_items { get; set; }
    }
}