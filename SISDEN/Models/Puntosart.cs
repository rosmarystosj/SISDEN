using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Puntosart
{
    public int Idpuntoart { get; set; }

    public string Puntoartnumero { get; set; } = null!;

    public string Puntoartdescripcion { get; set; } = null!;

    public int Idarticulo { get; set; }
}
