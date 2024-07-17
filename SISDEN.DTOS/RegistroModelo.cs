using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class RegistroModelo
    {
        [Required(ErrorMessage = "Este campo es requerido.")]

        public string? Usunombre { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]

        public string? Usuapellido { get; set; }

        [EmailAddress(ErrorMessage = "Digite un correo valido.")]
        [Required(ErrorMessage = "Este campo es requerido.")]

        public string? Usuemail { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]

        public string? Usuidentificacion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string? Usucontraseña { get; set; }

        public int Usurol { get; set; }
        public string? Usuverificacion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public string? Usutelefono { get; set; }
        public string? Usutelefono2 { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public int? Usuentidad { get; set; }
        public string? Usustatus { get; set; }
        

    }
}
