using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StylistShop.Core.Models;
using StylistShop.DataAccess.InMemory;
using StylistShop.Core.ViewModels;
using StylistShop.Core.Contracts;
using System.IO;

namespace StylistShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        //Create an instance of product repository
        //ProductRepository context;

        //Create product object using the generics created. Disabled last command
        //InMemoryRepository<Product> context;

        //Create an instance of productCategory repository
        // ProductCategoryRepository productCategories;
        
        //Create product category object using the generics created. Disabled last command 
        //InMemoryRepository<ProductCategory> productCategories;

        //Interface implementation, so we get rid of Generics
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        //Create a constructor to initialize repository
        //Passing a class that implements an IRepository product and an IRepository productCategory
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
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
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == true)
            {
                //Assign image to product to create
                if (file != null)
                {
                    //Assign product Id as the name in case user uploads files with same names
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
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
        public ActionResult Edit(Product product, string Id, HttpPostedFileBase file)
        {
            var productToEdit = context.Find(Id);
            if (product != null)
            {
                if (ModelState.IsValid)
                {
                    //Assign image to productToEdit
                    if (file != null)
                    {
                        //Assign product Id as the name in case user uploads files with same names
                        productToEdit.Image = product.Id + Path.GetExtension(file.FileName);
                        file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                    }

                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
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