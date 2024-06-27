using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Articulo
{
    public int Idarticulo { get; set; }

    public string Artnombre { get; set; } = null!;

    public string? Artdescripcion { get; set; }

    public int CapIdcapitulo { get; set; }
}
