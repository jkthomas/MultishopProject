using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multishop.Web.Models.CartViewModels
{
    public class ProductIndexViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public bool IsAdmin { get; set; }
    }
}