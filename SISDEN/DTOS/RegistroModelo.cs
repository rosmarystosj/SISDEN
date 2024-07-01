namespace SISDEN.DTOS
{
    public class RegistroModelo
    {

        public string Usunombre { get; set; }
        public string Usuapellido { get; set; }
        public string? Usuidentificacion { get; set; }
        public string? Usuverificacion { get; set; }

        public string? Usucontraseña { get; set; }

        public string? Usutelefono { get; set; }
        public string? Usutelefono2 { get; set; }
        public string? Usuemail { get; set; }
        public int? Usuentidad { get; set; }
        public string? Usustatus { get; set; }
        public int Usurol { get; set; }
    }
}
