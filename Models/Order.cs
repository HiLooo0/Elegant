// Models/Order.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elegant.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        // Зв'язок з таблицею користувачів ASP.NET Identity (може бути порожнім, якщо купують без реєстрації)
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // "Нове", "В обробці", "Відправлено", "Скасовано"

        [Required]
        public decimal TotalPrice { get; set; }

        // Навігаційна властивість: товари, що входять до цього замовлення
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}