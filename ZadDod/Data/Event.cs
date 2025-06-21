using System;
using System.Collections.Generic;

namespace ZadDod.Data;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public int MaxParticipants { get; set; }

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual ICollection<Speaker> Speakers { get; set; } = new List<Speaker>();
}
