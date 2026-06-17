using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Назва меблів")]
        public string Name { get; set; }

        [Display(Name = "Артикул")]
        public string Article { get; set; }

        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Display(Name = "Ціна")]
        public decimal Price { get; set; } // Використовуємо decimal для грошей

        [Display(Name = "Стара ціна (для акції)")]
        public decimal? OldPrice { get; set; }

        [Display(Name = "Зображення (URL)")]
        public string ImageUrl { get; set; }

        [Display(Name = "Кількість на складі")]
        public int Stock { get; set; }

        [Display(Name = "Хіт продажу")]
        public bool IsFeatured { get; set; }

        // Зв'язок з категорією
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}