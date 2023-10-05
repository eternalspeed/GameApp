using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? UserId { get; set; }

    public int? GameId { get; set; }

    public string? ReviewTitle { get; set; }

    public decimal? Score { get; set; }

    public string? Body { get; set; }

    public virtual Game? Game { get; set; }

    public virtual AspNetUser? User { get; set; }
}
