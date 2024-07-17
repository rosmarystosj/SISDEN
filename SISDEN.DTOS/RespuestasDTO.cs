namespace SISDEN.DTOS
{
    public class RespuestasDTO
    {
        public int Idrespuesta { get; set; }

        public string Respdescripcion { get; set; } = null!;

        public int RespIdpregunta { get; set; }

        public int? RespIdopcion { get; set; }

        public int RespIdusuario { get; set; }
    }
}
