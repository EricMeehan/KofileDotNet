using System.Collections.Generic;

namespace KofileDotNet.Models
{
    class DistResult
        {
            public string order_number {get; set;}
            public Dictionary<string, decimal> dist_totals {get; set;}
        }
}