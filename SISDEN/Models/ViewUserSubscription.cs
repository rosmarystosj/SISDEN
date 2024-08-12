using System;
using System.Collections.Generic;

namespace SISDEN.Models;

public partial class ViewUserSubscription
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public int SubscriptionId { get; set; }

    public string Endpoint { get; set; } = null!;

    public string P256dh { get; set; } = null!;

    public string Auth { get; set; } = null!;
}
