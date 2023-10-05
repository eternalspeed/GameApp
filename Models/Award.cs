using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Award
{
    public int AwardId { get; set; }

    public string? UserId { get; set; }

    public int? GameId { get; set; }

    public string? AwardName { get; set; }

    public virtual Game? Game { get; set; }

    public virtual AspNetUser? User { get; set; }
}
