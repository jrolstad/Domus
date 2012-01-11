using System.ComponentModel.DataAnnotations;

namespace Domus.Entities
{
    /// <summary>
    /// Person who uses the application
    /// </summary>
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

        /// <summary>
        /// The person's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The person's last name
        /// </summary>
        public string LastName { get; set; }
    }
}