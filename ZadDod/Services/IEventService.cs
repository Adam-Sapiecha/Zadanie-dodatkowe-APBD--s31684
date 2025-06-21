
using ZadDod.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZadDod.Services
{
    public interface IEventService
    {
        Task<int> CreateEventAsync(CreateEventDTO dto);
        Task AssignSpeakerAsync(AssignSpeakerDTO dto);
        Task RegisterParticipantAsync(RegisterParticipantDTO dto);
        Task CancelRegistrationAsync(int eventId, int participantId);
        Task<List<EventSummaryDTO>> GetUpcomingEventsAsync();
        Task<List<EventReportDTO>> GetParticipantReportAsync(int participantId);
    }
}
