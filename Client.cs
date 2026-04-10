using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessSystem.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        public string? Notes { get; set; }

        // Навигационные свойства
        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}