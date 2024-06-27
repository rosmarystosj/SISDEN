using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Seccion
{
    public int Idseccion { get; set; }

    public string Secnombre { get; set; } = null!;

    public int CapIdcapitulo { get; set; }
}
