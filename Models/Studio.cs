using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Studio
{
    public int StudioId { get; set; }

    public int? StudioRoleId { get; set; }

    public string? StudioName { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Game> Games { get; } = new List<Game>();

    public virtual StudioRole? StudioRole { get; set; }
}
