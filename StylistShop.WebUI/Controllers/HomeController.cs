using StylistShop.Core.Contracts;
using StylistShop.Core.Models;
using StylistShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StylistShop.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //Interface implementation, so we combine generics with interface for Product and category
        IRepository<Product> context;
        IRepository<ProductCategory> productCategories;

        //Create a constructor to initialize repository
        //Passing a class that implements an IRepository product and an IRepository productCategory
        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            context = productContext;
            productCategories = productCategoryContext;
        }
                                 //if no category is passed then display all products
        public ActionResult Index(string category = null)
        {
            //Send a list of products to the View
            List<Product> products;
            var categories = productCategories.Collection().ToList();
            if (category==null)
            {
                products = context.Collection().ToList();
            }
            else
            {
                products = context.Collection().Where(prod => prod.Category == category).ToList();
            }

            var productListModel = new ProductListViewModel();
            productListModel.Products = products;
            productListModel.ProductCategories = categories;

            return View(productListModel);
        }

        public ActionResult Details(string Id)
        {
            var product = context.Find(Id);
            if (product != null)
            {
                return View(product);
            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}