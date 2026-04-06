using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessSystem.Models
{
    public class GroupClass
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название занятия обязательно")]
        [Display(Name = "Название занятия")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Инструктор обязателен")]
        [Display(Name = "Инструктор")]
        public string Instructor { get; set; } = string.Empty;

        [Required(ErrorMessage = "День недели обязателен")]
        [Display(Name = "День недели")]
        public string DayOfWeek { get; set; } = string.Empty;

        [Required(ErrorMessage = "Время начала обязательно")]
        [DataType(DataType.Time)]
        [Display(Name = "Время начала")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Время окончания обязательно")]
        [DataType(DataType.Time)]
        [Display(Name = "Время окончания")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Максимальное количество участников")]
        [Range(1, 50, ErrorMessage = "Количество участников должно быть от 1 до 50")]
        public int MaxParticipants { get; set; } = 20;

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Стоимость")]
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

        [Display(Name = "Зал")]
        public string Room { get; set; } = "Основной зал";

        [Display(Name = "Активно")]
        public bool IsActive { get; set; } = true;

        // Навигационное свойство
        public virtual ICollection<GroupClassBooking> Bookings { get; set; } = new List<GroupClassBooking>();
    }

    public class GroupClassBooking
    {
        public int Id { get; set; }

        [Required]
        public int GroupClassId { get; set; }
        public virtual GroupClass? GroupClass { get; set; }

        [Required]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime ClassDate { get; set; }

        [Display(Name = "Посещено")]
        public bool IsAttended { get; set; } = false;

        public string? Notes { get; set; }
    }
}