using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Game
{
    public int GameId { get; set; }

    public int? SeriesId { get; set; }

    public int? StudioId { get; set; }

    public string? ImageUrl { get; set; }

    public string? GameTitle { get; set; }

    public string? Genere { get; set; }

    public string? Platform { get; set; }

    public string? Engine { get; set; }

    public string? Mode { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public decimal? Score { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Artwork> Artworks { get; } = new List<Artwork>();

    public virtual ICollection<Award> Awards { get; } = new List<Award>();

    public virtual ICollection<Cast> Casts { get; } = new List<Cast>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual Series? Series { get; set; }

    public virtual Studio? Studio { get; set; }
}
