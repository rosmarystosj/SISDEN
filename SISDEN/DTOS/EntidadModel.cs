using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class EntidadModel
    {
        [Required(ErrorMessage = "Este campo es requerido.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string? Usucontraseña { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public int? Usuentidad { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono debe contener solo números.")] 
        public string? Usutelefono { get; set; }

        public string? Usustatus { get; set; }

        public int Usurol { get; set; }
        public string? Usuverificacion { get; set; }

        public string? Usuemail { get; set; }

    }
}
