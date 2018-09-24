using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylistShop.Core.Models
{
    public class ProductCategory : BaseEntity
    {
        //Below command not needed as it is defined in the BaseEntity class
        //public string Id { get; set; }
        public string Category { get; set; }

        //No need for this constructor as the ID is generated in the BaseEntity constructor
        //public ProductCategory()
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}
    }
}
