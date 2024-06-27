using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaArticulo
{
    public int Idarticulo { get; set; }

    public string Artnombre { get; set; } = null!;

    public string? Artdescripcion { get; set; }

    public string CapituloNombre { get; set; } = null!;

    public string? SeccionNombre { get; set; }

    public string? Puntoartnumero { get; set; }

    public string? Puntoartdescripcion { get; set; }
}
