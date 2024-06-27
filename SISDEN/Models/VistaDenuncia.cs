using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class VistaDenuncia
{
    public int Iddenuncia { get; set; }

    public string Dentitulo { get; set; } = null!;

    public DateTime Denfechacreacion { get; set; }

    public DateTime? Denfechacierre { get; set; }

    public bool? Denevidenciaadjunta { get; set; }

    public string? Denobservaciones { get; set; }

    public int? Dennumprision { get; set; }

    public bool? Denprision { get; set; }

    public bool? Denservicio { get; set; }

    public int? Dennumserv { get; set; }

    public int? Dennumsalarios { get; set; }

    public bool? Densalarios { get; set; }

    public string? Estado { get; set; }

    public string? MotivoCierre { get; set; }

    public string? UsuarioNombre { get; set; }

    public string? UsuarioApellido { get; set; }

    public string? UsuarioTelefono { get; set; }

    public string? UsuarioEmail { get; set; }
}
