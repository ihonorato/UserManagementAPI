using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is mandatory.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is mandatory.")]
        public string Email { get; set; } = string.Empty;
    }
}
