using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KofileDotNet.Data;
using System.IO;
using KofileDotNet.Models;

namespace KofileDotNet.Controllers
{
    [Route("api/[controller]")]
    public class Part3Controller : Controller
    {
        
        [HttpPost, Route("Prices")]
        public Price[] Prices([FromBody] Order[] orderList)
        {
            if(orderList == null)
                return null;

            List<Price> prices = new List<Price>();

            foreach(Order order in orderList)
            {
                Price price = new Price() {
                    order_number = order.order_number,
                    items = new List<PriceItem>()
                };
                prices.Add(price);

                decimal total = 0;
                
                foreach(OrderItem orderItem in order.order_items)
                {
                    decimal cost = getPrice(orderItem);
                    total += cost;

                    price.items.Add(new PriceItem() {
                        type = orderItem.type,
                        price = cost
                    });
                }
                
                price.total = total;
            }

            return prices.ToArray();
        }

                
        [HttpPost, Route("Dist")]
        public dynamic Dist([FromBody] Order[] orderList)
        {
            if(orderList == null)
                return null;
            
            List<DistResult> dists = new List<DistResult>();

            foreach(Order order in orderList)
            {
                DistResult dist = new DistResult() {
                    order_number = order.order_number,
                    dist_totals = new Dictionary<string, decimal>()
                };
                
                dists.Add(dist);

                foreach(OrderItem orderItem in order.order_items)
                {
                    GetDistribution(dist.dist_totals, orderItem);
                }
            }

            return dists.ToArray();
        }


        [NonAction]
        decimal getPrice(OrderItem orderItem)
        {
            FeeType feeType = FeesDataReader.Instance.FeeList.Single(ft => ft.order_item_type == orderItem.type);

            decimal subtotal = 0;

            Fee flatFee = feeType.fees.FirstOrDefault(f => f.type == "flat");

            subtotal += Convert.ToDecimal(flatFee.amount);

            Fee pageFee = feeType.fees.FirstOrDefault(f => f.type == "per-page");

            if(pageFee != null && orderItem.pages > 1)
                subtotal += Convert.ToDecimal(pageFee.amount) * (orderItem.pages - 1);

            return subtotal;
        }
        
        [NonAction]
        void GetDistribution(Dictionary<string, decimal> distTotals, OrderItem orderItem)
        {
            FeeType feeType = FeesDataReader.Instance.FeeList.Single(ft => ft.order_item_type == orderItem.type);

            //Grab/Add in the distribution of fees
            foreach(Distribution d in feeType.distributions)
            {
                if(distTotals.ContainsKey(d.name))
                    distTotals[d.name] += Convert.ToDecimal(d.amount);
                else
                    distTotals[d.name] = Convert.ToDecimal(d.amount);
            }

            //Check if we need an "Other" Category
            decimal itemPrice = this.getPrice(orderItem);
            decimal itemSubTotal = feeType.distributions.Sum(d => Convert.ToDecimal(d.amount));
            if(itemSubTotal < itemPrice)
            {
                if(distTotals.ContainsKey("Other"))
                    distTotals["Other"] += itemPrice - itemSubTotal;
                else
                    distTotals["Other"] = itemPrice - itemSubTotal;
            }
        }
    }
}