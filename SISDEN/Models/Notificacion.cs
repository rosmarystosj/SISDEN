using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Notificacion
{
    public int Id { get; set; }

    public int? Idusuario { get; set; }

    public int? EntidadId { get; set; }

    public int Idestado { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime Fechaenvio { get; set; }

    public int Leido { get; set; }
}
