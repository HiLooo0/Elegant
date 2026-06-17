using System.Web;
using System.Web.Mvc;

namespace Elegant.Models
{
    public static class CartHelper
    {
        private const string CartSessionKey = "ElegantCart";

        public static Cart GetCart(this Controller controller)
        {
            var cart = controller.Session[CartSessionKey] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                controller.Session[CartSessionKey] = cart;
            }
            return cart;
        }

        public static void SaveCart(this Controller controller, Cart cart)
        {
            controller.Session[CartSessionKey] = cart;
        }
    }
}
