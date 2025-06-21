using System;


namespace ZadDod.DTOs;

public class EventSummaryDTO
{
    public int EventId { get; set; }
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public List<string> Speakers { get; set; } = new();
    public int RegisteredCount { get; set; }
    public int AvailableSpots { get; set; }
}
