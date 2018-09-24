using StylistShop.Core.Models;
using StylistShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StylistShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        // GET: ProductCategoryManager
        //Create an instance of productCategory repository
        ProductCategoryRepository context;

        //Create a constructor to initialize repository
        public ProductCategoryManagerController()
        {
            context = new ProductCategoryRepository();
        }

        public ActionResult Index()
        {
            //Get list of product categories from DB and pass them to view
            var productCategories = context.CollectionOfCategoryProducts().ToList();
            return View(productCategories);
        }

        //Controller to create new product
        public ActionResult Create()
        {
            var productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid == true)
            {
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }
            else
            {
                return View(productCategory);
            }
        }

        public ActionResult Edit(string Id)
        {
            var productCategory = context.Find(Id);
            if (productCategory != null)
            {
                return View(productCategory);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            var productCategoryToEdit = context.Find(Id);
            if (productCategory != null)
            {
                if (ModelState.IsValid)
                {
                    productCategoryToEdit.Category = productCategory.Category;
                    
                    context.Commit();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(productCategory);
                }
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(string Id)
        {
            var productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete != null)
            {
                return View(productCategoryToDelete);
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
            var productCategoryToDelete = context.Find(Id);
            if (productCategoryToDelete != null)
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