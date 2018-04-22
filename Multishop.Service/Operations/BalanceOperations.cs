using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Service.Operations
{
    public class BalanceOperations
    {
        public static bool CanBuy(List<OrderProduct> products, decimal balance)
        {
            decimal price = GetPrice(products);
            if (balance < price)
            {
                return false;
            }

            return true;
        }

        public static decimal GetPrice(List<OrderProduct> products)
        {
            decimal price = 0;
            foreach (OrderProduct product in products)
            {
                price += product.Product.UnitPrice * product.Quantity;
            }

            return price;
        }
    }
}
