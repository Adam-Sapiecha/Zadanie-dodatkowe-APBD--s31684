
using System;
using System.ComponentModel.DataAnnotations;

namespace ZadDod.DTOs;

    public class CreateEventDTO
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Date { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxParticipants { get; set; }
    }