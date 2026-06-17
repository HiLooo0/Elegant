using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Elegant.Models
{
    public class WarehouseEntryItem
    {
        public int Id { get; set; }

        [Display(Name = "Кількість")]
        public int Quantity { get; set; }

        // Зв'язок з документом, до якого належить цей рядок
        public int WarehouseEntryId { get; set; }
        public virtual WarehouseEntry WarehouseEntry { get; set; }

        // Зв'язок з товаром (меблями)
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}