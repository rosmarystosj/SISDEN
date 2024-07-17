namespace SISDEN.DTOS
{
    public class SesionDTO
    {
        public string Sesionid { get; set; } = null!;

        public string telefono { get; set; } = null!;

        public DateTime? Createat { get; set; }

        public DateTime? Expiresat { get; set; }
    }
}
