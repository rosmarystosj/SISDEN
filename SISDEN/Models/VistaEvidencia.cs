using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaEvidencia
{
    public int Idevidencia { get; set; }

    public string Evurl { get; set; } = null!;

    public string? TipoEvidencia { get; set; }

    public int? Iddenuncia { get; set; }

    public int? Idtipoevid { get; set; }
    public string? DenunciaTitulo { get; set; }
}
