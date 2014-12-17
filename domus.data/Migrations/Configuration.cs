using System.Collections.Generic;
using domus.data.models;

namespace domus.data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<domus.data.DomusContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(domus.data.DomusContext context)
        {
            var categories = new List<Category>
            {
                new Category{Id = 1,Name = "Appetizers "},
                new Category{Id = 2,Name = "Beef "},
                new Category{Id = 3,Name = "Beverages "},
                new Category{Id = 4,Name = "Breakfast "},
                new Category{Id = 5,Name = "Cake "},
                new Category{Id = 6,Name = "Chicken "},
                new Category{Id = 7,Name = "Christmas Cookies "},
                new Category{Id = 8,Name = "Cookies "},
                new Category{Id = 9,Name = "Cookies Done "},
                new Category{Id = 10,Name = "Desserts "},
                new Category{Id = 11,Name = "Dog Treats "},
                new Category{Id = 12,Name = "Events "},
                new Category{Id = 13,Name = "Fish "},
                new Category{Id = 14,Name = "Frosting "},
                new Category{Id = 15,Name = "Lamb "},
                new Category{Id = 16,Name = "Pasta "},
                new Category{Id = 17,Name = "Pies "},
                new Category{Id = 18,Name = "Pork "},
                new Category{Id = 19,Name = "Quick Breads "},
                new Category{Id = 20,Name = "Salads "},
                new Category{Id = 21,Name = "Sauces "},
                new Category{Id = 22,Name = "Side Dishes "},
                new Category{Id = 23,Name = "Soups "},
                new Category{Id = 24,Name = "Turkey "},
                new Category{Id = 25,Name = "Vegetable Dishes "},
                new Category{Id = 26,Name = "Yeast Breads "},
            };

            context.Categories.AddOrUpdate(categories.ToArray());
           
        }
    }
}
