// Файл: Models/WarehouseOutputItem.cs
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class WarehouseOutputItem
    {
        public int Id { get; set; }

        [Display(Name = "Кількість")]
        public int Quantity { get; set; }

        public int WarehouseOutputId { get; set; }
        public virtual WarehouseOutput WarehouseOutput { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
