using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylistShop.Core.Models
{
    //Product class inherits from BaseEntity
    public class Product : BaseEntity
    {
        //Removed below line as BaseEntiry class already have an ID
        //public string Id { get; set; }

        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }

        //We do not need this constructor as the ID now is generated in the BaseEntity class
        //public Product ()
        //{
        //    //Generate Id per product
        //    this.Id = Guid.NewGuid().ToString();
        //}
    }
}
