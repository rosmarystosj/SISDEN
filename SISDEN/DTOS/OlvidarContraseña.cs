using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class OlvidarContraseña
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
