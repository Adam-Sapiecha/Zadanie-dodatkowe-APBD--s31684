
using ZadDod.DTOs;
using ZadDod.Services;
using Microsoft.AspNetCore.Mvc;

namespace ZadDod.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;

        public EventsController(IEventService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventDTO dto)
        {
            var id = await _service.CreateEventAsync(dto);
            return CreatedAtAction(nameof(CreateEvent), new { id }, null);
        }

        [HttpPost("assign-speaker")]
        public async Task<IActionResult> AssignSpeaker(AssignSpeakerDTO dto)
        {
            await _service.AssignSpeakerAsync(dto);
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterParticipant(RegisterParticipantDTO dto)
        {
            await _service.RegisterParticipantAsync(dto);
            return NoContent();
        }

        [HttpDelete("cancel")]
        public async Task<IActionResult> CancelRegistration(int eventId, int participantId)
        {
            await _service.CancelRegistrationAsync(eventId, participantId);
            return NoContent();
        }

        [HttpGet("upcoming")]
        public async Task<ActionResult<List<EventSummaryDTO>>> GetUpcoming()
        {
            return Ok(await _service.GetUpcomingEventsAsync());
        }

        [HttpGet("report/{participantId}")]
        public async Task<ActionResult<List<EventReportDTO>>> GetReport(int participantId)
        {
            return Ok(await _service.GetParticipantReportAsync(participantId));
        }
    }
}
