using StylistShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StylistShop.Core.Contracts
{
    public interface IBasketService
    {
        void AddItemToBasket(HttpContextBase httpContext, string productId);
        void RemoveItemFromBasket(HttpContextBase httpContext, string itemId);
        List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext);
        BasketSummaryViewModel GetBasketsummary(HttpContextBase httpContext);
    }
}
