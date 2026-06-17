namespace Elegant.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Elegant.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Elegant.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            // ===== Категорії =====
            var lighting = new Elegant.Models.Category { Name = "Освітлення" };
            var furniture = new Elegant.Models.Category { Name = "Меблі" };
            var decor = new Elegant.Models.Category { Name = "Декор" };
            var textile = new Elegant.Models.Category { Name = "Текстиль" };

            context.Categories.AddOrUpdate(c => c.Name, lighting, furniture, decor, textile);
            context.SaveChanges();

            // ===== Товари =====
            context.Products.AddOrUpdate(p => p.Article,
                new Elegant.Models.Product
                {
                    Article = "LMP-001",
                    Name = "Підвісний світильник Loft",
                    Description = "Металевий підвісний світильник у стилі лофт",
                    Price = 1450,
                    OldPrice = 1800,
                    ImageUrl = "https://images.unsplash.com/photo-1540932239986-30128078f3c5?q=80&w=600&auto=format&fit=crop",
                    Stock = 12,
                    IsFeatured = true,
                    CategoryId = lighting.Id
                },
                new Elegant.Models.Product
                {
                    Article = "LMP-002",
                    Name = "Настільна лампа Bronx",
                    Description = "Настільна лампа з тканинним абажуром",
                    Price = 980,
                    ImageUrl = "https://images.unsplash.com/photo-1507473885765-e6ed057f782c?q=80&w=600&auto=format&fit=crop",
                    Stock = 8,
                    IsFeatured = true,
                    CategoryId = lighting.Id
                },
                new Elegant.Models.Product
                {
                    Article = "SOF-001",
                    Name = "Диван Manhattan",
                    Description = "Тришарований диван з оббивкою з вельвету",
                    Price = 24990,
                    OldPrice = 28990,
                    ImageUrl = "https://images.unsplash.com/photo-1555041469-a586c61ea9bc?q=80&w=600&auto=format&fit=crop",
                    Stock = 4,
                    IsFeatured = true,
                    CategoryId = furniture.Id
                },
                new Elegant.Models.Product
                {
                    Article = "CHR-001",
                    Name = "Крісло Velvet",
                    Description = "Оксамитове крісло на металевих ніжках",
                    Price = 6890,
                    ImageUrl = "https://images.unsplash.com/photo-1567538096631-e0c55bd6374c?q=80&w=600&auto=format&fit=crop",
                    Stock = 6,
                    IsFeatured = true,
                    CategoryId = furniture.Id
                },
                new Elegant.Models.Product
                {
                    Article = "TBL-001",
                    Name = "Журнальний столик Oak",
                    Description = "Столик з масиву дуба та металевими ніжками",
                    Price = 4250,
                    ImageUrl = "https://images.unsplash.com/photo-1601057199127-d6c0f7f5b3a3?q=80&w=600&auto=format&fit=crop",
                    Stock = 10,
                    CategoryId = furniture.Id
                },
                new Elegant.Models.Product
                {
                    Article = "DEC-001",
                    Name = "Ваза керамічна Terra",
                    Description = "Декоративна ваза ручної роботи",
                    Price = 650,
                    ImageUrl = "https://images.unsplash.com/photo-1578500494198-246f612d3b3d?q=80&w=600&auto=format&fit=crop",
                    Stock = 15,
                    IsFeatured = true,
                    CategoryId = decor.Id
                },
                new Elegant.Models.Product
                {
                    Article = "DEC-002",
                    Name = "Дзеркало настінне Round",
                    Description = "Кругле дзеркало в металевій рамі",
                    Price = 1890,
                    ImageUrl = "https://images.unsplash.com/photo-1618220179428-22790b461013?q=80&w=600&auto=format&fit=crop",
                    Stock = 7,
                    CategoryId = decor.Id
                },
                new Elegant.Models.Product
                {
                    Article = "TXT-001",
                    Name = "Плед вовняний Cozy",
                    Description = "М'який плед з натуральної шерсті",
                    Price = 1200,
                    OldPrice = 1450,
                    ImageUrl = "https://images.unsplash.com/photo-1580301762395-83f8c8a8c9a4?q=80&w=600&auto=format&fit=crop",
                    Stock = 20,
                    IsFeatured = true,
                    CategoryId = textile.Id
                },
                new Elegant.Models.Product
                {
                    Article = "TXT-002",
                    Name = "Подушка декоративна Linen",
                    Description = "Лляна подушка з прихованою застібкою",
                    Price = 450,
                    ImageUrl = "https://images.unsplash.com/photo-1592789705501-f9ae4287c4cb?q=80&w=600&auto=format&fit=crop",
                    Stock = 25,
                    CategoryId = textile.Id
                }
            );

            context.SaveChanges();

            // ===== Адміністратор =====
            if (!context.Users.Any(u => u.Email.ToLower() == "elegant@gmail.com"))
            {
                context.Users.Add(new Elegant.Models.User
                {
                    Email = "elegant@gmail.com",
                    FullName = "Адміністратор",
                    PasswordHash = System.Web.Helpers.Crypto.HashPassword("Elegant2025!"),
                    CreatedAt = System.DateTime.Now,
                    IsAdmin = true
                });
                context.SaveChanges();
            }
        }
    }
}
