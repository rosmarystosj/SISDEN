using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Sesion
{
    public int Id { get; set; }

    public string Sesionid { get; set; } = null!;

    public string Userid { get; set; } = null!;

    public DateTime? Createat { get; set; }

    public DateTime? Expiresat { get; set; }
}
