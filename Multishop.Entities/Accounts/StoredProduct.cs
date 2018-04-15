using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Multishop.Entities.Accounts;

namespace Multishop.Entities.ShopEntities
{
    public class StoredProduct
    {
        [Key]
        public int StoredProductId { get; set; }
        public string InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }



        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
