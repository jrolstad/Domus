using System.ComponentModel.DataAnnotations;
using Directus.SimpleDb.Attributes;

namespace Domus.Entities
{
    /// <summary>
    /// Person who uses the application
    /// </summary>
    [DomainName("Domus_User")]
    public class User
    {
        /// <summary>
        /// Name of the user
        /// </summary>
        [Key]
        public string Email { get; set; }

        /// <summary>
        /// Password for the user
        /// </summary>
        public string Password { get; set; }
    }
}