using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaViolacione
{
    public int Idviolacion { get; set; }

    public string? Artnombre { get; set; }

    public string? Artdescripcion { get; set; }

    public string? Puntoartnumero { get; set; }

    public string? Puntoartdescripcion { get; set; }

    public int? Iddenuncia { get; set; }

    public string? Dentitulo { get; set; }
}
