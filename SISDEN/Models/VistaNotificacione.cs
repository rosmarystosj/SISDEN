using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaNotificacione
{
    public int Idnotificacion { get; set; }

    public int Idusuario { get; set; }

    public string? UsuarioNombreCompleto { get; set; }

    public string? Usuemail { get; set; }

    public int Idestado { get; set; }

    public string? EstadoDescripcion { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime Fechaenvio { get; set; }

    public bool? Leido { get; set; }
}
