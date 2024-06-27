using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Comentario
{
    public int Idcomentario { get; set; }

    public string Comdescripcion { get; set; } = null!;

    public int ComIdusuario { get; set; }

    public int ComIddenuncia { get; set; }
}
