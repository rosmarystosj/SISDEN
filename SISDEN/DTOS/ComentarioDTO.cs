namespace SISDEN.DTOS
{
    public class ComentarioDTO
    {
        public int Idcomentario { get; set; }

        public string Comdescripcion { get; set; } = null!;

        public int? ComIdusuario { get; set; }

        public int ComIddenuncia { get; set; }

        public int? ComIdrol { get; set; }
        public DateTime? ComFecha { get; set; }

    }
}
