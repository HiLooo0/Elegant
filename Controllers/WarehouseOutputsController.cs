// Файл: Controllers/WarehouseOutputsController.cs
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Elegant.Models;
using Elegant.Filters;

namespace Elegant.Controllers
{

    [AdminOnly]
    public class WarehouseOutputsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.WarehouseOutputs.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            WarehouseOutput warehouseOutput = db.WarehouseOutputs
                .Include(e => e.Items.Select(i => i.Product))
                .FirstOrDefault(e => e.Id == id);

            if (warehouseOutput == null) return HttpNotFound();

            var viewModel = new WarehouseOutputDetailViewModel
            {
                WarehouseOutput = warehouseOutput,
                ProductsSelectList = db.Products.ToList().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),
                NewOutputItem = new WarehouseOutputItem { WarehouseOutputId = warehouseOutput.Id }
            };

            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OutputDate,Destination")] WarehouseOutput warehouseOutput)
        {
            if (ModelState.IsValid)
            {
                db.WarehouseOutputs.Add(warehouseOutput);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = warehouseOutput.Id });
            }
            return View(warehouseOutput);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(WarehouseOutputItem newOutputItem)
        {
            if (ModelState.IsValid)
            {
                // Розраховуємо поточний залишок
                int totalIn = db.WarehouseEntryItems.Where(i => i.ProductId == newOutputItem.ProductId).Sum(i => (int?)i.Quantity) ?? 0;
                int totalOut = db.WarehouseOutputItems.Where(i => i.ProductId == newOutputItem.ProductId).Sum(i => (int?)i.Quantity) ?? 0;
                int currentStock = totalIn - totalOut;

                // Перевіряємо, чи достатньо товару
                if (newOutputItem.Quantity > currentStock)
                {
                    TempData["ErrorMessage"] = $"Недостатньо товару на складі! Доступно: {currentStock} шт.";
                }
                else
                {
                    db.WarehouseOutputItems.Add(newOutputItem);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id = newOutputItem.WarehouseOutputId });
        }

        public ActionResult DeleteItem(int id)
        {
            var item = db.WarehouseOutputItems.Find(id);
            if (item != null)
            {
                var outputId = item.WarehouseOutputId;
                db.WarehouseOutputItems.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = outputId });
            }
            return HttpNotFound();
        }

        public ActionResult Delete(int id)
        {
            WarehouseOutput warehouseOutput = db.WarehouseOutputs.Find(id);
            if (warehouseOutput == null) return HttpNotFound();

            var itemsToDelete = db.WarehouseOutputItems.Where(i => i.WarehouseOutputId == id);
            db.WarehouseOutputItems.RemoveRange(itemsToDelete);
            db.WarehouseOutputs.Remove(warehouseOutput);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
