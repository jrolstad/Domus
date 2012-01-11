using System;

namespace Domus.Entities
{
    /// <summary>
    /// Domain model for a recipe Category
    /// </summary>
    [Serializable]
    public class Category
    {
        /// <summary>
        /// Description of the Category
        /// </summary>
        public string Description { get; set; }
    }
}
