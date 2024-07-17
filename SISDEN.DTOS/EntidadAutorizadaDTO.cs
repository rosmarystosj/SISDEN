namespace SISDEN.DTOS
{
    public class EntidadAutorizadaDTO
    {
        public int Identidadaut { get; set; }

        public string Entautorizadadescp { get; set; } = null!;

        public string EntidadSector { get; set; } = null!;

        public string? EntautorizadaEstatus { get; set; }

        public string EntCorreo { get; set; } = null!;

    }
}
