using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using StylistShop.Core.Models;

namespace StylistShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        //Constructor
        public ProductRepository()
        {
            //Look in the cache if there is a list of products
            products = cache["products"] as List<Product>;
            // If no products in cache then create a list of products
            if (products==null)
            {
                products = new List<Product>();
            }
        }

        //Method to save the new products in the cache before saving them into the DB
        public void Commit()
        {
            cache["products"] = products;
        }

        public void Insert(Product product)
        {
            products.Add(product);
        }

        public void Update(Product product)
        {
            //Look for product in DB, if found it then update otherwise display error
            Product productToUpdate = products.Find(p => p.Id == product.Id);
            
            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public Product Find(string Id)
        {
            //Look for product in DB, if found then return it otherwise display error
            Product product = products.Find(p => p.Id == Id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        //Return a list of products
        public IQueryable<Product> CollectionOfProducts()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            //Look for product in DB, if found it then delete otherwise display error
            Product productToDelete = products.Find(p => p.Id == Id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
    }

}
