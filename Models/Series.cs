using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GameApp.Models;

public partial class Series
{
    public int SeriesId { get; set; }

    public string? SeriesName { get; set; }

    public int? NumberOfTitles { get; set; }

    public virtual ICollection<Game> Games { get; } = new List<Game>();
}
