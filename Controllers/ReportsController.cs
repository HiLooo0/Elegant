// Файл: Controllers/ReportsController.cs
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Elegant.Models;
using Elegant.Filters;

namespace Elegant.Controllers
{
    [AdminOnly]
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports/Stock
        public ActionResult Stock()
        {
            // Отримуємо список всіх товарів з нашого асортименту
            var allProducts = db.Products.ToList();

            var stockReport = new List<StockReportViewModel>();

            // Проходимо по кожному товару і розраховуємо його залишок
            foreach (var product in allProducts)
            {
                // 1. Рахуємо, скільки всього товару НАДІЙШЛО
                int totalIn = db.WarehouseEntryItems
                                .Where(item => item.ProductId == product.Id)
                                .Sum(item => (int?)item.Quantity) ?? 0;

                // 2. Рахуємо, скільки всього товару БУЛО ВІДВАНТАЖЕНО
                int totalOut = db.WarehouseOutputItems
                                 .Where(item => item.ProductId == product.Id)
                                 .Sum(item => (int?)item.Quantity) ?? 0;

                // 3. Розраховуємо кінцевий залишок
                int currentStock = totalIn - totalOut;

                stockReport.Add(new StockReportViewModel
                {
                    ProductId = product.Id,
                    ProductArticle = product.Article,
                    ProductName = product.Name,
                    CurrentStock = currentStock
                });
            }

            // Передаємо готовий звіт на сторінку для відображення
            return View(stockReport.OrderBy(r => r.ProductName).ToList());
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