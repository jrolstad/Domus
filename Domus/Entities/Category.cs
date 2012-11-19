using System;
using System.ComponentModel.DataAnnotations;
using Directus.SimpleDb.Attributes;

namespace Domus.Entities
{
    /// <summary>
    /// Domain model for a recipe Category
    /// </summary>
    [Serializable]
    [DomainName("Domus_Category")]
    public class Category
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        [Key]
        public string CategoryId { get; set; }

        /// <summary>
        /// Description of the Category
        /// </summary>
        public string Description { get; set; }
    }
}
