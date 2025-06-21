using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZadDod.Data;
using ZadDod.DTOs;
using ZadDod.Exceptions;

namespace ZadDod.Services
{
    public class EventService : IEventService
    {
        private readonly AppDBContext _context;

        public EventService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<int> CreateEventAsync(CreateEventDTO dto)
        {
            if (dto.Date < DateTime.Now)
                throw new BadRequestException("Event date must be in the future.");

            var ev = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                MaxParticipants = dto.MaxParticipants
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev.Id;
        }

        public async Task AssignSpeakerAsync(AssignSpeakerDTO dto)
        {
            var ev = await _context.Events
                .Include(e => e.Speakers)
                .FirstOrDefaultAsync(e => e.Id == dto.EventId);

            if (ev == null)
                throw new NotFoundException("Event not found.");

            var speaker = await _context.Speakers.FindAsync(dto.SpeakerId);
            if (speaker == null)
                throw new NotFoundException("Speaker not found.");

            bool isBusy = await _context.Events
                .AnyAsync(e => e.Id != ev.Id &&
                               e.Date == ev.Date &&
                               e.Speakers.Any(s => s.Id == dto.SpeakerId));

            if (isBusy)
                throw new BadRequestException("Speaker is already assigned to another event at the same time.");

            ev.Speakers.Add(speaker);
            await _context.SaveChangesAsync();
        }

        public async Task RegisterParticipantAsync(RegisterParticipantDTO dto)
        {
            var ev = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == dto.EventId);

            if (ev == null)
                throw new NotFoundException("Event not found.");

            if (ev.Participants.Count >= ev.MaxParticipants)
                throw new BadRequestException("Event is full.");

            bool alreadyRegistered = ev.Participants.Any(p => p.Id == dto.ParticipantId);
            if (alreadyRegistered)
                throw new BadRequestException("Participant already registered for this event.");

            var participant = await _context.Participants.FindAsync(dto.ParticipantId);
            if (participant == null)
                throw new NotFoundException("Participant not found.");

            ev.Participants.Add(participant);
            await _context.SaveChangesAsync();
        }

        public async Task CancelRegistrationAsync(int eventId, int participantId)
        {
            var ev = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                throw new NotFoundException("Event not found.");

            if ((ev.Date - DateTime.Now).TotalHours < 24)
                throw new BadRequestException("Cannot cancel registration less than 24 hours before event.");

            var participant = ev.Participants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
                throw new NotFoundException("Participant is not registered for this event.");

            ev.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }

        public async Task<List<EventSummaryDTO>> GetUpcomingEventsAsync()
        {
            return await _context.Events
                .Where(e => e.Date >= DateTime.Now)
                .Include(e => e.Speakers)
                .Include(e => e.Participants)
                .Select(e => new EventSummaryDTO
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Date = e.Date,
                    Speakers = e.Speakers.Select(s => s.Name).ToList(),
                    RegisteredCount = e.Participants.Count,
                    AvailableSpots = e.MaxParticipants - e.Participants.Count
                })
                .ToListAsync();
        }

        public async Task<List<EventReportDTO>> GetParticipantReportAsync(int participantId)
        {
            return await _context.Events
                .Where(e => e.Participants.Any(p => p.Id == participantId))
                .Include(e => e.Speakers)
                .Select(e => new EventReportDTO
                {
                    EventTitle = e.Title,
                    EventDate = e.Date,
                    SpeakerNames = e.Speakers.Select(s => s.Name).ToList()
                })
                .ToListAsync();
        }
    }
}
