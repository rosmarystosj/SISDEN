using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class ContactoDTO
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string? UsunombreCompleto { get; set; }


        [EmailAddress(ErrorMessage = "Digite un correo valido.")]
        [Required(ErrorMessage = "El correo electrónico es requerido.")]

        public string? Usuemail { get; set; }

        [Required(ErrorMessage = "Ingrese un mensaje.")]

        public string? Mensaje { get; set; }
        [Required(ErrorMessage = "El teléfono es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono debe contener solo números.")]

        public string? Usutelefono { get; set; }
    }
}
