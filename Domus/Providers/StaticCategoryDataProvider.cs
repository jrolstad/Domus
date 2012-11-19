using System;
using System.Collections.Generic;
using System.Linq;
using Domus.Entities;

namespace Domus.Providers
{
    public class StaticCategoryDataProvider:IDataProvider<Category,string>
    {
        private static readonly IEnumerable<string> _availableCategories =
            new List<string>
                {
                    "Appetizers",
                    "Beverages",
                    "Quick Breads",
                    "Yeast Breads",
                    "Cake",
                    "Frosting",
                    "Cookies",
                    "Pies",
                    "Desserts",
                    "Fish",
                    "Beef",
                    "Chicken",
                    "Pork",
                    "Soups",
                    "Vegetable Dishes",
                    "Side Dishes",
                    "Breakfast",
                    "Pasta",
                    "Salads",
                    "Sauces",
                    "Dog Treats",
                    "Turkey",
                    "Lamb",
                    "Christmas Cookies",
                    "Christmas Cookies Done"
                };
        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Category Get( string identifier )
        {
            return _availableCategories
                .AsParallel()
                .Where(c => c == identifier)
                .Select(c => new Category {Description = c})
                .FirstOrDefault();
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Category> Get()
        {
            return _availableCategories
                .AsParallel()
                .Select(c => new Category {Description = c})
                .OrderBy(c=>c.Description);
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Category> Search( Func<Category, bool> filterCriteria )
        {
            return _availableCategories
                .AsParallel()
                .Select(c => new Category {Description = c})
                .Where(filterCriteria);
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        public void Save(Category item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a given item
        /// </summary>
        /// <param name="identifier"></param>
        public void Delete( string identifier )
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}