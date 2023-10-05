using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class CastRole
{
    public int CastRoleId { get; set; }

    public string? CastRoleName { get; set; }

    public virtual ICollection<Cast> Casts { get; } = new List<Cast>();
}
