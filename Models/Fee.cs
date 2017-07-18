using System.Collections.Generic;

namespace KofileDotNet.Models
{
    public class Fee
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
    }

    public class Distribution
    {
        public string name { get; set; }
        public string amount { get; set; }
    }

    public class FeeType
    {
        public string order_item_type { get; set; }
        public List<Fee> fees { get; set; }
        public List<Distribution> distributions { get; set; }
    }
}