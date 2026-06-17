using System.Linq;
using System.Web.Mvc;
using Elegant.Models;
using Elegant.Filters;

namespace Elegant.Controllers
{
    [AdminOnly]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.ProductsCount = db.Products.Count();
            ViewBag.CategoriesCount = db.Categories.Count();
            ViewBag.OrdersCount = db.Orders.Count();
            ViewBag.NewOrdersCount = db.Orders.Count(o => o.Status == "Нове");
            ViewBag.ReviewsCount = db.Reviews.Count();
            ViewBag.LowStockCount = db.Products.Count(p => p.Stock <= 3);

            ViewBag.RecentOrders = db.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToList();

            return View();
        }

        // GET: Admin/Reviews
        public ActionResult Reviews()
        {
            var reviews = db.Reviews
                .Include("Product")
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
            return View(reviews);
        }

        // POST: Admin/DeleteReview/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteReview(int id)
        {
            var review = db.Reviews.Find(id);
            if (review != null)
            {
                db.Reviews.Remove(review);
                db.SaveChanges();
            }
            return RedirectToAction("Reviews");
        }

        // GET: Admin/Orders
        public ActionResult Orders()
        {
            var orders = db.Orders
                .Include("OrderItems.Product")
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(orders);
        }

        // POST: Admin/UpdateOrderStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderStatus(int id, string status)
        {
            var order = db.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Orders");
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
