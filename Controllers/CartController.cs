using System;
using System.Linq;
using System.Web.Mvc;
using Elegant.Models;

namespace Elegant.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.ActivePage = "cart";
            var cart = this.GetCart();
            return View(cart);
        }

        // POST: Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            var cart = this.GetCart();
            cart.AddItem(product, quantity);
            this.SaveCart(cart);

            return RedirectToAction("Index");
        }

        // POST: Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int productId, int quantity)
        {
            var cart = this.GetCart();
            cart.UpdateQuantity(productId, quantity);
            this.SaveCart(cart);
            return RedirectToAction("Index");
        }

        // POST: Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(int productId)
        {
            var cart = this.GetCart();
            cart.RemoveItem(productId);
            this.SaveCart(cart);
            return RedirectToAction("Index");
        }

        // GET: Cart/Checkout
        public ActionResult Checkout()
        {
            var cart = this.GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            ViewBag.ActivePage = "cart";
            return View(new Order { OrderDate = DateTime.Now });
        }

        // POST: Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "CustomerName,Phone,Address")] Order order)
        {
            var cart = this.GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            ModelState.Remove("OrderDate");
            ModelState.Remove("Status");
            ModelState.Remove("TotalPrice");

            if (!ModelState.IsValid)
            {
                ViewBag.ActivePage = "cart";
                return View(order);
            }

            order.OrderDate = DateTime.Now;
            order.Status = "Нове";
            order.TotalPrice = cart.Total;
            order.UserId = User != null && User.Identity != null ? User.Identity.Name : null;

            db.Orders.Add(order);
            db.SaveChanges();

            foreach (var item in cart.Items)
            {
                db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }
            db.SaveChanges();

            cart.Clear();
            this.SaveCart(cart);

            return RedirectToAction("Success", new { id = order.Id });
        }

        // GET: Cart/Success/5
        public ActionResult Success(int id)
        {
            ViewBag.ActivePage = "cart";
            ViewBag.OrderId = id;
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
