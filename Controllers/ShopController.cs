using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Elegant.Models;
using PagedList;

namespace Elegant.Controllers
{
    public class ShopController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Shop (Головна сторінка магазину)
        public ActionResult Index()
        {
            ViewBag.ActivePage = "home";

            ViewBag.Categories = db.Categories.ToList();

            // Хіти продажу — товари з позначкою IsFeatured, або просто перші 8
            var featured = db.Products
                .Include(p => p.Category)
                .Where(p => p.IsFeatured)
                .Take(8)
                .ToList();

            if (!featured.Any())
            {
                featured = db.Products
                    .Include(p => p.Category)
                    .OrderByDescending(p => p.Id)
                    .Take(8)
                    .ToList();
            }

            return View(featured);
        }

        // GET: Shop/Catalog
        public ActionResult Catalog(int? categoryId, string sort, int? page)
        {
            ViewBag.ActivePage = "catalog";
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.Sort = sort;

            var products = db.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            switch (sort)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name":
                    products = products.OrderBy(p => p.Name);
                    break;
                default:
                    products = products.OrderByDescending(p => p.Id);
                    break;
            }

            int pageSize = 12;
            int pageNumber = page ?? 1;

            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Shop/Product/5
        public ActionResult Product(int id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.Reviews = db.Reviews
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            ViewBag.Related = db.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id)
                .Take(4)
                .ToList();

            return View(product);
        }

        // POST: Shop/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(int productId, int rating, string comment)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            string authorName = Request.IsAuthenticated ? User.Identity.Name : "Гість";

            if (rating < 1) rating = 1;
            if (rating > 5) rating = 5;

            db.Reviews.Add(new Review
            {
                ProductId = productId,
                AuthorName = authorName,
                Rating = rating,
                Comment = comment,
                CreatedAt = System.DateTime.Now
            });
            db.SaveChanges();

            return RedirectToAction("Product", new { id = productId });
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
