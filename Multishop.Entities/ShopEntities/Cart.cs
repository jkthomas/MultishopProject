using Multishop.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multishop.Entities.ShopEntities
{
    public class Cart
    {
        [ForeignKey("User")]
        public string CartId { get; set; }

        
        public virtual ApplicationUser User { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
