
using System;
using System.ComponentModel.DataAnnotations;

namespace RescueRoute.Models
{
    public class EmergencyRequest
    {
        public int Id { get; set; }

        [Required]
        public string RequesterId { get; set; } = "";

        [Required]
        public string AssistanceNeeded { get; set; } = "";

        [Required]
        public string Location { get; set; } = "";

        [Required]
        public string Urgency { get; set; } = "Medium";

        [Required]
        public string Details { get; set; } = "";

        public string Status { get; set; } = "Pending Review";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

