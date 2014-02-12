using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Domus.Web.Models.Api;
using FizzWare.NBuilder;

namespace Domus.Web.Controllers
{
    public class RecipeApiController : ApiController
    {
        private static IList<RecipeApiModel> _recipeData;

        public RecipeApiController()
        {
            if(_recipeData == null)
                _recipeData = Builder<RecipeApiModel>
                    .CreateListOfSize(20)
                    .All()
                    .Do(r=>r.ImageUrl = null)
                    .Build();
        }

        public IEnumerable<RecipeApiModel> Get()
        {
            return _recipeData;
        }

        public RecipeApiModel Get(string id)
        {
            return _recipeData.FirstOrDefault(r => r.RecipeId == id);
        }

        public void Post([FromBody]RecipeApiModel value)
        {
            if (!_recipeData.Any(r => r.RecipeId == value.RecipeId))
            {
                _recipeData.Add(value);
            }
            
            if (_recipeData.Any(r => r.RecipeId == value.RecipeId))
            {
                var existing = _recipeData.First(r => r.RecipeId == value.RecipeId);
                _recipeData.Remove(existing);

                _recipeData.Add(value);
            }
        }

        public void Put(string id, [FromBody]RecipeApiModel value)
        {
            if (!_recipeData.Any(r => r.RecipeId == id))
            {
                _recipeData.Add(value);
            }

            if (_recipeData.Any(r => r.RecipeId == id))
            {
                var existing = _recipeData.First(r => r.RecipeId == id);
                _recipeData.Remove(existing);

                _recipeData.Add(value);
            }
        }

        public void Delete(string id)
        {
            if (_recipeData.Any(r => r.RecipeId == id))
            {
                var existing = _recipeData.First(r => r.RecipeId == id);
                _recipeData.Remove(existing);
            }
        }


    }
}
