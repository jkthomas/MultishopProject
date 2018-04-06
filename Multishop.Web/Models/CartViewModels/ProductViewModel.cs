using Multishop.Entities.ShopEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Multishop.Web.Models.CartViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}