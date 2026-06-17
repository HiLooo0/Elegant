// Файл: Models/WarehouseOutputDetailViewModel.cs
using System.Collections.Generic;
using System.Web.Mvc;

namespace Elegant.Models
{
    public class WarehouseOutputDetailViewModel
    {
        public WarehouseOutput WarehouseOutput { get; set; }
        public IEnumerable<SelectListItem> ProductsSelectList { get; set; }
        public WarehouseOutputItem NewOutputItem { get; set; }
    }
}