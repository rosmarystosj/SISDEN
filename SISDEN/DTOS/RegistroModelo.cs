using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class RegistroModelo
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string? Usunombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido.")]

        public string? Usuapellido { get; set; }

        [EmailAddress(ErrorMessage = "Digite un correo valido.")]
        [Required(ErrorMessage = "El correo electrónico es requerido.")]

        public string? Usuemail { get; set; }

        [Required(ErrorMessage = "La cédula de identidad es requerida.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "La cédula de identidad debe contener solo 11 números.")]

        public string? Usuidentificacion { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string? Usucontraseña { get; set; }

        public int Usurol { get; set; }
        public string? Usuverificacion { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono debe contener solo números.")]

        public string? Usutelefono { get; set; }
        public string? Usutelefono2 { get; set; }

        public string? Usustatus { get; set; }
        

    }
}
