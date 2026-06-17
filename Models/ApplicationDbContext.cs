using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity; // Цей рядок важливий для DbContext

namespace Elegant.Models // Перевірте, чи назва вашого проєкту правильна
{
    // Цей клас є нашим мостом між моделями C# та базою даних
    public class ApplicationDbContext : DbContext
    {
        // Конструктор, який вказує, яку строку підключення використовувати з Web.config
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        // Тут ми перераховуємо всі наші моделі, які мають стати таблицями в БД
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WarehouseEntry> WarehouseEntries { get; set; }
        public DbSet<WarehouseEntryItem> WarehouseEntryItems { get; set; }
        public DbSet<WarehouseOutput> WarehouseOutputs { get; set; }
        public DbSet<WarehouseOutputItem> WarehouseOutputItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
    }
}