using StylistShop.Core.Contracts;
using StylistShop.Core.Models;
using StylistShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StylistShop.Services
{
    public class BasketService : IBasketService
    {
        IRepository<Product> _productContext;
        IRepository<Basket> _basketContext;
        //Cannot be updated anywhere else in the code but here
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            this._productContext = productContext;
            this._basketContext = basketContext;
        }

        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie basketCookie = httpContext.Request.Cookies.Get(BasketSessionName);
            var basket = new Basket();
            //if user has visited the page before and added items to her basket then get BasketId
            //Otherwise create a new basket
            if (basketCookie != null)
            {
                string basketId = basketCookie.Value;
                if (!string.IsNullOrEmpty(basketId))
                {
                    //Get basket Id from DB
                    basket = _basketContext.Find(basketId);
                }
                else
                {
                    if (createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }

            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }
            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            //Create new basket object. Remember Id is automatically created when object is created
            var basket = new Basket();
            //Save basket in DB
            _basketContext.Insert(basket);
            _basketContext.Commit();
            //Create a new cookie
            HttpCookie basketCookie = new HttpCookie(BasketSessionName);
            //Assign basket Id to cookie newly created. so we can identify it later as needed
            basketCookie.Value = basket.Id;
            //Add expiration date
            basketCookie.Expires = DateTime.Now.AddDays(1);
            //Send cookie back to the user
            httpContext.Response.Cookies.Add(basketCookie);

            return basket;
        }

        public void AddItemToBasket(HttpContextBase httpContext, string productId)
        {
            //Before adding an item we need to verify that basket exist 
            //Otherwise create a new one
            var basket = GetBasket(httpContext, true);
            //Load basket items for this user from DB
            var item = basket.BasketItems.FirstOrDefault(itemm => itemm.ProductId == productId);
            //If item do not exist in basket then create and add it to basket. 
            //Otherwise added to existing basket with items
            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity += 1;
            }
            //Save it to DB
            _basketContext.Commit();
        }

        public void RemoveItemFromBasket(HttpContextBase httpContext, string itemId)
        {
            //Get basket items from DB
            var basket = GetBasket(httpContext, true);
            //Look for itemId in basket list returned from DB
            var item = basket.BasketItems.FirstOrDefault(itemm => itemm.Id == itemId);
            //If item found, then remove it from basket 
            if (item != null)
            {
                basket.BasketItems.Remove(item);
                _basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            //Get basket from DB, passing "false" as we don't want to create a new basket
            //If no items in basket then return the in memory empty basket 
            var basket = GetBasket(httpContext, false);
            //Return items in basket, otherwise return an empty basket
            if (basket != null)
            {
             var results = (from bas in basket.BasketItems
                           join pro in _productContext.Collection() on bas.ProductId equals pro.Id
                           select new BasketItemViewModel()
                           {
                               Id = bas.Id,
                               Quantity = bas.Quantity,
                               ProductName = pro.Name,
                               Image = pro.Image,
                               Price = pro.Price
                           }).ToList();

             return results;
            }
            else
            {
                return new List<BasketItemViewModel>();
            }

        }

        //Display how many items and total in basket
        public BasketSummaryViewModel GetBasketsummary(HttpContextBase httpContext)
        {
            var _basket = GetBasket(httpContext, false);
            var _basketSummary = new BasketSummaryViewModel(0, 0);

            if (_basket != null)
            {
                //? means variable accepts null
                int? basketCount = (from item in _basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in _basket.BasketItems
                                        join pro in _productContext.Collection() on item.ProductId equals pro.Id
                                        select item.Quantity * pro.Price).Sum();
               
                //if basketCount/basketTotal is null assign a 0, otherwise assign value returned
                _basketSummary.BasketCount = basketCount ?? 0;
                _basketSummary.BasketTotal = basketTotal ?? decimal.Zero;

                return _basketSummary;
            }
            else
            {
                return _basketSummary;
            }
            
        }

    }
}
