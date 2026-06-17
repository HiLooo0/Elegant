using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Назва категорії")] // Так буде відображатись у формах
        public string Name { get; set; }
    }
}