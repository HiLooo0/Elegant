// Файл: Controllers/WarehouseEntriesController.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Elegant.Models; // Змінено на назву вашого проєкту
using Elegant.Filters;

namespace Elegant.Controllers // Змінено на назву вашого проєкту
{
    [AdminOnly]
    public class WarehouseEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WarehouseEntries
        public ActionResult Index()
        {
            return View(db.WarehouseEntries.ToList());
        }

        // GET: WarehouseEntries/Details/5
        // ЦЕЙ МЕТОД ПОВНІСТЮ ПЕРЕПИСАНО
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Знаходимо накладну і підвантажуємо пов'язані з нею товари та їх назви
            WarehouseEntry warehouseEntry = db.WarehouseEntries
                .Include(e => e.Items.Select(i => i.Product))
                .FirstOrDefault(e => e.Id == id);

            if (warehouseEntry == null)
            {
                return HttpNotFound();
            }

            // Створюємо ViewModel, яка буде містити всі дані для нашої сторінки
            var viewModel = new WarehouseEntryDetailViewModel
            {
                WarehouseEntry = warehouseEntry,
                // Готуємо список меблів для випадаючого меню
                ProductsSelectList = db.Products.ToList().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),
                // Готуємо порожній об'єкт для форми додавання
                NewEntryItem = new WarehouseEntryItem
                {
                    WarehouseEntryId = warehouseEntry.Id
                }
            };

            return View(viewModel);
        }

        // GET: WarehouseEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WarehouseEntries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EntryDate,Source")] WarehouseEntry warehouseEntry)
        {
            if (ModelState.IsValid)
            {
                db.WarehouseEntries.Add(warehouseEntry);
                db.SaveChanges();
                // ЗМІНЕНО: Тепер перенаправляємо не на список, а на деталі щойно створеної накладної
                return RedirectToAction("Details", new { id = warehouseEntry.Id });
            }

            return View(warehouseEntry);
        }

        // НОВИЙ МЕТОД: для додавання товару в накладну
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(WarehouseEntryItem newEntryItem)
        {
            if (ModelState.IsValid)
            {
                db.WarehouseEntryItems.Add(newEntryItem);
                db.SaveChanges();
            }
            // Повертаємося на сторінку деталей, щоб побачити доданий товар
            return RedirectToAction("Details", new { id = newEntryItem.WarehouseEntryId });
        }


        // GET: WarehouseEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WarehouseEntry warehouseEntry = db.WarehouseEntries.Find(id);
            if (warehouseEntry == null)
            {
                return HttpNotFound();
            }
            return View(warehouseEntry);
        }

        // POST: WarehouseEntries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EntryDate,Source")] WarehouseEntry warehouseEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(warehouseEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(warehouseEntry);
        }


        // POST: WarehouseEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WarehouseEntry warehouseEntry = db.WarehouseEntries.Find(id);
            db.WarehouseEntries.Remove(warehouseEntry);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // НОВИЙ МЕТОД: для видалення товару з накладної
        public ActionResult DeleteItem(int id)
        {
            var item = db.WarehouseEntryItems.Find(id);
            if (item != null)
            {
                var entryId = item.WarehouseEntryId;
                db.WarehouseEntryItems.Remove(item);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = entryId });
            }
            return HttpNotFound();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: WarehouseEntries/Delete/5
        // ЦЕЙ МЕТОД ТЕПЕР ОДРАЗУ ВИДАЛЯЄ ЗАПИС
        public ActionResult Delete(int id)
        {
            WarehouseEntry warehouseEntry = db.WarehouseEntries.Find(id);
            if (warehouseEntry == null)
            {
                return HttpNotFound();
            }

            // ВАЖЛИВО: Спочатку видаляємо всі пов'язані товари в накладній
            var itemsToDelete = db.WarehouseEntryItems.Where(i => i.WarehouseEntryId == id);
            db.WarehouseEntryItems.RemoveRange(itemsToDelete);

            // Потім видаляємо саму накладну
            db.WarehouseEntries.Remove(warehouseEntry);

            db.SaveChanges(); // Зберігаємо всі зміни в базі даних

            return RedirectToAction("Index"); // Повертаємо користувача до оновленого списку
        }
    }
}