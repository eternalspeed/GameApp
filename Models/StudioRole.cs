using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class StudioRole
{
    public int StudioRoleId { get; set; }

    public string? StudioRoleName { get; set; }

    public virtual ICollection<Studio> Studios { get; } = new List<Studio>();
}
