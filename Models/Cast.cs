using System;
using System.Collections.Generic;

namespace GameApp.Models;

public partial class Cast
{
    public int CastId { get; set; }

    public int? CastRoleId { get; set; }

    public int? GameId { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Sex { get; set; }

    public int? Age { get; set; }

    public virtual CastRole? CastRole { get; set; }

    public virtual Game? Game { get; set; }
}
