using System;
using System.Collections.Generic;

namespace ZadDod.Data;

public partial class Participant
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
