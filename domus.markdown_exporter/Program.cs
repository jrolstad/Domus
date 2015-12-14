using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domus.Entities;
using Domus.Providers.Cacheing;
using Domus.Providers.Repositories;

namespace domus.markdown_exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var recipeRepository = new AmazonSimpleDbRecipeProvider("AKIAIPPUKG4ROEJABARA", "u2M5frbU506uI3gQtwil8bWzdiam0bDYx0BZAgpB", new InMemoryCache());

            var recipes = recipeRepository.Get();

            foreach (var recipe in recipes)
            {
                var recipeText = ConvertToMarkdown(recipe);

                var fileName = GetRecipeFileName(recipe);
                var filePath = Path.Combine(@"D:\github\jrolstad\jrolstad.github.io\_posts", fileName);
                File.WriteAllText(filePath, recipeText);
            }
        }

        private static string GetRecipeFileName(Recipe recipe)
        {
            var recipeName = recipe.Name.Trim()
                .Replace(" ", "-")
                .Replace(@"/", "-")
                .Replace(@"\", "-")
                .Replace(":", "-")
                .Replace("\"", "-")
                .Replace("?", "-")
                .ToLower();

            var fileName = string.Format("{0:yyyy-MM-dd}-{1}.md", DateTime.Today, recipeName);
            return fileName;
        }

        private static string ConvertToMarkdown(Recipe recipe)
        {
            string recipeFormat = @"---
published: true
title: {0}
layout: post
categories: [{1}]
rating: {2}
---
### Servings
{3}

### Ingredients
{4}

### Directions
{5}

### Source
{6}
";
            var servings = GetServings(recipe);
            var source = GetSource(recipe);
            var ingredients = GetIngredients(recipe);
            var directions = GetDirections(recipe);
            return string.Format(recipeFormat,recipe.Name,recipe.Category,recipe.Rating,servings,ingredients,directions,source);
        }

        private static string GetDirections(Recipe recipe)
        {
            if (recipe.Directions == null) return null;

            var directionLines = recipe.Directions
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var formattedDirectionLines = new List<string>();

            var lineNumber = 1;
            foreach (var line in directionLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                else
                {
                    var cleanedUpLine = line
                      .Trim();

                    for (var i = 0; i < 20; i++)
                    {
                        cleanedUpLine = cleanedUpLine.Replace(string.Format("{0}.",i), "");
                        cleanedUpLine = cleanedUpLine.Replace(string.Format("{0})",i), "");
                        cleanedUpLine = cleanedUpLine.Trim();
                    }

                    if (string.IsNullOrWhiteSpace(cleanedUpLine))
                        continue;

                    var formattedLine = string.Format("{0}. {1}", lineNumber, cleanedUpLine);
                    formattedDirectionLines.Add(formattedLine);
                    lineNumber++;
                }
            }

            var directions = string.Join(Environment.NewLine, formattedDirectionLines);
            return directions;
        }



        private static string GetIngredients(Recipe recipe)
        {
            if (recipe.Ingredients == null) return null;

            var ingredientLines = recipe.Ingredients
                .Split(new[] {Environment.NewLine}, StringSplitOptions.None)
                .Select(r => string.IsNullOrWhiteSpace(r) ? r : string.Format("- {0}", r))
                .ToList();

            var ingredients = string.Join(Environment.NewLine, ingredientLines);
            return ingredients;
        }

        private static string GetServings(Recipe recipe)
        {
            if (recipe.Servings == null || recipe.Servings.Trim() == "0")
                return "";

            return recipe.Servings;
        }

        private static string GetSource(Recipe recipe)
        {
            if (recipe.Source == null) return null;

            if (recipe.Source.StartsWith("http:"))
                return string.Format("<a href=\"{0}\" target=\"new\">{0}</a>",recipe.Source);

            return recipe.Source;
        }
    }
}
