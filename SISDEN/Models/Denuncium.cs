using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Denuncium
{
    public int Iddenuncia { get; set; }

    public string? Dentitulo { get; set; }

    public string? Dendescripcion { get; set; }

    public string? Denanimal { get; set; }

    public DateTime? Denfechacreacion { get; set; }

    public DateTime? Denfechacierre { get; set; }

    public bool? Denevidenciaadjunta { get; set; }

    public bool? Denservicio { get; set; }

    public bool? Densalarios { get; set; }

    public bool? Denprision { get; set; }

    public int? Dennumsalarios { get; set; }

    public int? Dennumprision { get; set; }

    public int? Dennumserv { get; set; }

    public string? Denobservaciones { get; set; }

    public int? DenIdmotivocierre { get; set; }

    public string? Denubicacion { get; set; }

    public int? DenIdusuario { get; set; }

    public int? DenIdestado { get; set; }

    public string? Densesion { get; set; }

    public string? Dencategoria { get; set; }
}
