using System;

namespace ZadDod.DTOs;

public class EventReportDTO
{
    public string EventTitle { get; set; }
    public DateTime EventDate { get; set; }
    public List<string> SpeakerNames { get; set; } = new();
}
