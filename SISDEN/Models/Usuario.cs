using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string Usunombre { get; set; } = null!;

    public string Usuapellido { get; set; } = null!;

    public string? Usuidentificacion { get; set; }

    public string? Usutelefono { get; set; }

    public string? Usutelefono2 { get; set; }

    public string? Usuemail { get; set; }

    public int? Usuentidad { get; set; }

    public string? Usustatus { get; set; }

    public int Usurol { get; set; }
}
