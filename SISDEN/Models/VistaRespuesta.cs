using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaRespuesta
{
    public int Idrespuesta { get; set; }

    public string Respdescripcion { get; set; } = null!;

    public string? PreguntaDescripcion { get; set; }

    public string? OpcionDescripcion { get; set; }

    public string? UsuarioNombre { get; set; }

    public string? UsuarioApellido { get; set; }
}
