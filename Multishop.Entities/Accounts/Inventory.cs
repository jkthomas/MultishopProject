using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multishop.Entities.ShopEntities;

namespace Multishop.Entities.Accounts
{
    public class Inventory
    {
        [ForeignKey("User")]
        public string InventoryId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
