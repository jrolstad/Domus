using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Domus.Web.Models.Api;

namespace Domus.Web.Controllers
{
    public class CategoryApiController : ApiController
    {
        // GET api/categoryapi
        public IEnumerable<CategoryApiModel> Get()
        {
            return new[]
                {
                    new CategoryApiModel{Decription = "Appetizers"},
                    new CategoryApiModel{Decription = "Beef"},
                    new CategoryApiModel{Decription = "Beverages"}, 
                    new CategoryApiModel{Decription = "Breakfast"}, 
                    new CategoryApiModel{Decription = "Cake"}, 
                    new CategoryApiModel{Decription = "Chicken"}, 
                    new CategoryApiModel{Decription = "Christmas Cookies"}, 
                    new CategoryApiModel{Decription = "Cookies"},
                    new CategoryApiModel{Decription = "Desserts"}, 
                    new CategoryApiModel{Decription = "Dog Treats"}, 
                    new CategoryApiModel{Decription = "Events"}, 
                    new CategoryApiModel{Decription = "Fish"}, 
                    new CategoryApiModel{Decription = "Frosting"}, 
                    new CategoryApiModel{Decription = "Lamb"}, 
                    new CategoryApiModel{Decription = "Pasta"}, 
                    new CategoryApiModel{Decription = "Pies"}, 
                    new CategoryApiModel{Decription = "Pork"}, 
                    new CategoryApiModel{Decription = "Quick Breads"}, 
                    new CategoryApiModel{Decription = "Salads"}, 
                    new CategoryApiModel{Decription = "Sauces"}, 
                    new CategoryApiModel{Decription = "Side Dishes"}, 
                    new CategoryApiModel{Decription = "Soups"}, 
                    new CategoryApiModel{Decription = "Turkey"}, 
                    new CategoryApiModel{Decription = "Vegetable Dishes"}, 
                    new CategoryApiModel{Decription = "Yeast Breads"}"},
                };
        }

    }
}
