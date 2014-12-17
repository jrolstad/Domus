using System;
using System.ComponentModel.DataAnnotations;

namespace domus.data.models
{
    public class Recipe
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string ExternalRecipeId { get; set; }

        public virtual string Name { get; set; }

        public virtual int? Servings { get; set; }

        public virtual int? Rating { get; set; }

        public virtual int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual string Ingredients { get; set; }

        public virtual string Directions { get; set; }

        public virtual string Source { get; set; }

        public virtual bool IsDeleted { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
