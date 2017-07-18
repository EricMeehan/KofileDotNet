using System.Collections.Generic;

namespace KofileDotNet.Models
{
        public class PriceItem
        {
            public string type { get; set; }
            public decimal price { get; set; }
        }

        public class Price
        {
            public string order_number { get; set; }
            public List<PriceItem> items { get; set; }
            public decimal total { get; set; }
        }
}