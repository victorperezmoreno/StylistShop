using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StylistShop.Core.Models;
using StylistShop.DataAccess.InMemory;
using StylistShop.Core.ViewModels;

namespace StylistShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //Create an instance of product repository
        //ProductRepository context;

        //Create product object using the generics created. Disabled last command
        InMemoryRepository<Product> context;

        //Create an instance of productCategory repository
        // ProductCategoryRepository productCategories;
        
        //Create product category object using the generics created. Disabled last command 
        InMemoryRepository<ProductCategory> productCategories;   

        //Create a constructor to initialize repository
        public ProductManagerController()
        {
            context = new InMemoryRepository<Product>();
            productCategories = new InMemoryRepository<ProductCategory>();
        }
        
        // GET: ProductManager
        public ActionResult Index()
        {
            //Get list of products from DB and pass them to view
            var products = context.Collection().ToList();
            return View(products);
        }

        //Controller to create new product
        public ActionResult Create()
        {
            //Create an instance of product viewmodel
            var productViewModel = new ProductManagerViewModel();
            //Add an instance of product
            productViewModel.Product = new Product();
            //Populate categories field
            productViewModel.ProductCategories = productCategories.Collection();

            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid == true)
            {
                context.Insert(product);
                context.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Edit(string Id)
        {
            var product = context.Find(Id);
            if (product != null)
            {
                //Create an instance of product viewmodel
                var productViewModel = new ProductManagerViewModel();
                //Add an instance of product
                productViewModel.Product = product;
                //Populate categories field
                productViewModel.ProductCategories = productCategories.Collection();

                return View(productViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            var productToEdit = context.Find(Id);
            if (product != null)
            {
                if (ModelState.IsValid)
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    context.Commit();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(product);
                }         
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(string Id)
        {
            var productToDelete = context.Find(Id);
            if (productToDelete != null)
            {
                return View(productToDelete);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            var productToDelete = context.Find(Id);
            if (productToDelete != null)
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

    }
}