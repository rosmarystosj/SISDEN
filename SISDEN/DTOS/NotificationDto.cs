namespace SISDEN.DTOS
{
    public class NotificationDto
    {
        public int EntityId { get; set; }
        public int Idusuario {  get; set; }
        public int EntidadId { get; set; }
        public int Idestado { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fechaenvio { get; set; }
        public int Leido { get; set; }
        public string EventType { get; set; } // Tipo de evento que dispara la notificación
    }
}
