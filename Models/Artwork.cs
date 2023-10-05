using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Artwork
{
    public int ArtworkId { get; set; }

    public string? UserId { get; set; }

    public int? GameId { get; set; }

    public string? ArtworkUrl { get; set; }

    public string? ArtworkTitle { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public virtual Game? Game { get; set; }

    public virtual AspNetUser? User { get; set; }
}
