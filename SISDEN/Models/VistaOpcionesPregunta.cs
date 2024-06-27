using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaOpcionesPregunta
{
    public int Idopcionpreg { get; set; }

    public string Opcionpregdescripcion { get; set; } = null!;

    public string? PreguntaDescripcion { get; set; }

    public string? NumeroArticulo { get; set; }

    public string? PuntoArticuloDescripcion { get; set; }

    public string? Artnombre { get; set; }

    public string? Artdescripcion { get; set; }
}
