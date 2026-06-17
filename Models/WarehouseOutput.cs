// Файл: Models/WarehouseOutput.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Elegant.Models;
namespace Elegant.Models
{
    public class WarehouseOutput
    {
        public int Id { get; set; }

        [Display(Name = "Дата відвантаження")]
        public DateTime OutputDate { get; set; }

        [Display(Name = "Призначення")] // Наприклад, "Клієнт Іванов" або "Магазин №2"
        public string Destination { get; set; }

        public virtual ICollection<WarehouseOutputItem> Items { get; set; }
    }
}