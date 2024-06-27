using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Ubicacion
{
    public int Idubicacion { get; set; }

    public string? Ubdescripcion { get; set; }

    public decimal? Ublatitud { get; set; }

    public decimal? Ublongitud { get; set; }
}
