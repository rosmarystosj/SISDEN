using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Evidencium
{
    public int Idevidencia { get; set; }

    public string Evurl { get; set; } = null!;

    public int EvIdtipoevid { get; set; }

    public int EvIddenuncia { get; set; }
}
