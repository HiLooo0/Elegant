using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Elegant.Models
{
    public class WarehouseEntryDetailViewModel
    {
        // Сама накладна, яку ми переглядаємо
        public WarehouseEntry WarehouseEntry { get; set; }

        // Список всіх меблів для випадаючого списку у формі додавання
        public IEnumerable<SelectListItem> ProductsSelectList { get; set; }

        // Модель для нової позиції, яку ми будемо додавати
        public WarehouseEntryItem NewEntryItem { get; set; }
    }
}