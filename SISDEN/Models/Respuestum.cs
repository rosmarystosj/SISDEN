using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Respuestum
{
    public int Idrespuesta { get; set; }

    public string Respdescripcion { get; set; } = null!;

    public int RespIdpregunta { get; set; }

    public int? RespIdopcion { get; set; }

    public string RespIdusuario { get; set; } = null!;
}
