using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaUsuario
{
    public int Idusuario { get; set; }

    public string? Usunombre { get; set; }

    public string? Usuapellido { get; set; }

    public string? Usuidentificacion { get; set; }

    public string? Usutelefono { get; set; }

    public string? Usucontraseña { get; set; }

    public string? Usutelefono2 { get; set; }

    public string? Usuemail { get; set; }

    public string? Usustatus { get; set; }

    public int? Usurol { get; set; }

    public string? Entautorizadadescp { get; set; }
}
