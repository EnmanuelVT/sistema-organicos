using System.ComponentModel.DataAnnotations;

namespace ENTIDAD.DTOs.Users
{
    public class UserDto
    {
        public required string Id { get; set; }

        [Required]
        [StringLength(256)]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Role { get; set; }

        public string? UsCedula { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
    }
}