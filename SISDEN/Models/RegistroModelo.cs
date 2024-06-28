namespace SISDEN.Models
{
    public class RegistroModelo
    {

        public string Usunombre { get; set; }
        public string Usuapellido { get; set; }
        public string? Usuidentificacion { get; set; }
        public string? Usutelefono { get; set; }
        public string? Usutelefono2 { get; set; }
        public string? Usuemail { get; set; }
        public int? Usuentidad { get; set; }
        public string? Usustatus { get; set; }
        public int Usurol { get; set; }

        public bool IsDenunciante => Usurol == 1;
        public bool IsEntidad => Usurol == 2;
    }
}
