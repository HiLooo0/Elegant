using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class StockReportViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "Артикул")]
        public string ProductArticle { get; set; }

        [Display(Name = "Назва товару")]
        public string ProductName { get; set; }

        [Display(Name = "Поточний залишок")]
        public int CurrentStock { get; set; }
    }
}