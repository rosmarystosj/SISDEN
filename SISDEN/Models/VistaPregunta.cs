using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaPregunta
{
    public int Idpregunta { get; set; }

    public string Pregdescripcion { get; set; } = null!;

    public string? PreguntaTipo { get; set; }

    public string? Categoria { get; set; }
}
