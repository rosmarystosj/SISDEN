using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Opcionpreguntum
{
    public int Idopcionpreg { get; set; }

    public string Opcionpregdescripcion { get; set; } = null!;

    public int? OpcionpregIdpregunta { get; set; }

    public int? OpcionpregIdpuntoart { get; set; }

    public int? OpcionpregIdarticulo { get; set; }
}
