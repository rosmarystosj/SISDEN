using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Leyviolacion
{
    public int Idviolacion { get; set; }

    public int? ViolIdpuntoart { get; set; }

    public int? ViolIdarticulo { get; set; }

    public int ViolIddenuncia { get; set; }
}
