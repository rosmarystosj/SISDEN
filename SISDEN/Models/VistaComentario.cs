using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaComentario
{
    public int Idcomentario { get; set; }

    public string Comdescripcion { get; set; } = null!;

    public string? UsuarioNombre { get; set; }

    public string? UsuarioApellido { get; set; }

    public int? Iddenuncia { get; set; }

    public string? Rolnombre { get; set; }

    public string? DenunciaTitulo { get; set; }
}
