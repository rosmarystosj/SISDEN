using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Preguntum
{
    public int Idpregunta { get; set; }

    public string Pregdescripcion { get; set; } = null!;

    public int? PreIdcategoria { get; set; }

    public int PregTipo { get; set; }
}
