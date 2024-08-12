using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class Pushsubscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Endpoint { get; set; } = null!;

    public string P256dh { get; set; } = null!;

    public string Auth { get; set; } = null!;
}
