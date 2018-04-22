using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multishop.Web.Models.CartViewModels
{
    public class CartViewModel
    {
        public List<OrderProduct> OrderProducts { get; set; }
        public decimal Price { get; set; }
    }
}