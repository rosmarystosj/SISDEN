using System.ComponentModel.DataAnnotations;



namespace SISDEN.DTOS
{
    public class VerificarEmail
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]

        public string Codigo { get; set; }
    }
}
     