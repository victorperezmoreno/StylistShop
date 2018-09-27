using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylistShop.Core.Models
{
    public class Basket : BaseEntity
    {
        //virtual indicates that is is a lazy loading. Load all the items in basket
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        //Constructor
        public Basket()
            {
            this.BasketItems = new List<BasketItem>();
            }

    }
}
