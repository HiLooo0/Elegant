using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class WarehouseEntry
    {
        public int Id { get; set; }

        [Display(Name = "Дата надходження")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Джерело")] // Наприклад, "Виробничий цех"
        public string Source { get; set; }

        // Зв'язок з рядками документа (в одній накладній може бути багато товарів)
        public virtual ICollection<WarehouseEntryItem> Items { get; set; }
    }
}