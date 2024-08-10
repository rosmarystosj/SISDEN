using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class OlvidarContraseñaDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
