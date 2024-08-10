using System.ComponentModel.DataAnnotations;

namespace SISDEN.DTOS
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Este campo es requerido.")]

        public string? Usuidentificacion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public string? Usucontraseña { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        public string? Usuemail { get; set; }
        public int? UsuRol { get; set; }


    }
}
