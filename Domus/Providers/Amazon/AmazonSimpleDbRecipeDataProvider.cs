using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.SimpleDB;
using Amazon.SimpleDB.Model;
using Domus.Entities;
using Rolstad.Extensions;
using Attribute = Amazon.SimpleDB.Model.Attribute;

namespace Domus.Providers.Amazon
{
    public class AmazonSimpleDbRecipeDataProvider: IDataProvider<Recipe,string>
    {
        /// <summary>
        /// Domain where recipe data resides
        /// </summary>
        internal static string RecipeDomainName = "RecipeBookMVC_Recipe";

        /// <summary>
        /// Client for accessing SimpleDB data
        /// </summary>
        private readonly AmazonSimpleDB _simpleDBClient;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AmazonSimpleDbRecipeDataProvider(string accessKey, string secretKey)
        {
            _simpleDBClient = AWSClientFactory.CreateAmazonSimpleDBClient(accessKey, secretKey);
        }

        /// <summary>
        /// Obtains a single item
        /// </summary>
        /// <param name="identifier">Identifier to get the item for</param>
        /// <returns></returns>
        public Recipe Get( string identifier )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtains all of the items
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> GetAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search for a particular item
        /// </summary>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public IEnumerable<Recipe> Search( Func<Recipe, bool> filterCriteria )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a particular item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="identifier"></param>
        public void Save( Recipe item, string identifier )
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

        /// <summary>
        /// Obtains all recipe items from the SimpleDB
        /// </summary>
        /// <returns></returns>
        private List<Item> GetRecipesFromSimpleDb()
        {
            var isComplete = false;
            string nextToken = null;
            var recipes = new List<Item>();

            // Run the select many times until there are no more to get
            while (!isComplete)
            {
                // Setup the select to use the next token if defined
                var selectExpression = "Select * From {0} Where IsDeleted_0 != 'True'".StringFormat(RecipeDomainName);
                var request = new SelectRequest().WithSelectExpression(selectExpression).WithNextToken(nextToken);

                // Run the select
                var response = _simpleDBClient.Select(request);

                // Determine if we need to keep on going
                isComplete = !response.SelectResult.IsSetNextToken();
                nextToken = response.SelectResult.NextToken;

                // Add the results
                recipes.AddRange(response.SelectResult.Item);
            }

            return recipes;
        }

        /// <summary>
        /// Converts from a list of <c>SimpleDB</c> items (rows) to a enumeration of recipes
        /// </summary>
        /// <param name="recipeItems">Items to convert</param>
        /// <returns></returns>
        internal virtual IEnumerable<Recipe> Convert(IEnumerable<Item> recipeItems)
        {
            var recipes = recipeItems.Select(this.Convert);

            return recipes;
        }

        /// <summary>
        /// Converts a <c>SimpleDB</c> item to a recipe
        /// </summary>
        /// <param name="recipeItem">Item to convert</param>
        /// <returns></returns>
        internal virtual Recipe Convert(Item recipeItem)
        {
            var recipe = this.Convert(recipeItem.Attribute, recipeItem.Name);

            return recipe;
        }

        /// <summary>
        /// Converts a <c>SimpleDB</c> item to a recipe
        /// </summary>
        /// <param name="attributes">Attributes to convert</param>
        /// <param name="recipeId">Recipe identifier</param>
        /// <returns></returns>
        internal virtual Recipe Convert(IEnumerable<Attribute> attributes, string recipeId)
        {
            // Convert to a dictionary for easy searching
            var attributeDictionary = attributes.ToLookup(a => a.Name.Substring(0, a.Name.IndexOf("_")));

            // Create the recipe
            var recipe = new Recipe
            {
                RecipeId = recipeId,
                Name = attributeDictionary["Name"].GetFullValue(),
                Servings = attributeDictionary["Servings"].GetFullValue(),
                Rating = attributeDictionary["Rating"].GetFullValue().To<int>(),
                Category = attributeDictionary["Category"].GetFullValue(),
                Ingredients = attributeDictionary["Ingredients"].GetFullValue(),
                Directions = attributeDictionary["Directions"].GetFullValue(),
                Source = attributeDictionary["Source"].GetFullValue(),
                IsDeleted = attributeDictionary["IsDeleted"].GetFullValue().To<bool?>().GetValueOrDefault(),
            };

            return recipe;
        }

        /// <summary>
        /// Converts from a recipe to an array of replaceable attributes
        /// </summary>
        /// <param name="recipe">Recipe to convert</param>
        /// <returns></returns>
        public virtual ReplaceableAttribute[] Convert(Recipe recipe)
        {
            var replaceableAttributes = new List<ReplaceableAttribute>();

            replaceableAttributes.AddRange(recipe.Name.Convert("Name"));
            replaceableAttributes.AddRange(recipe.Servings.Convert("Servings"));
            replaceableAttributes.AddRange(recipe.Rating.ToString().Convert("Rating"));
            replaceableAttributes.AddRange(recipe.Category.Convert("Category"));
            replaceableAttributes.AddRange(recipe.Directions.Convert("Directions"));
            replaceableAttributes.AddRange(recipe.Ingredients.Convert("Ingredients"));
            replaceableAttributes.AddRange(recipe.Source.Convert("Source"));
            replaceableAttributes.AddRange(recipe.IsDeleted.ToString().Convert("IsDeleted"));

            return replaceableAttributes.ToArray();
        }

    }
}