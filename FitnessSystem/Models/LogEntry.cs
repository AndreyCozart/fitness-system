using System.ComponentModel.DataAnnotations;

namespace FitnessSystem.Models
{
    public class LogEntry
    {
        public int Id { get; set; }

        [Display(Name = "Пользователь")]
        public string? Username { get; set; }

        [Display(Name = "Действие")]
        public string Action { get; set; } = string.Empty;

        [Display(Name = "Контроллер")]
        public string? Controller { get; set; }

        [Display(Name = "Детали")]
        public string? Details { get; set; }

        [Display(Name = "IP адрес")]
        public string? IpAddress { get; set; }

        [Display(Name = "Дата и время")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [Display(Name = "Тип")]
        public string LogType { get; set; } = "Info";
    }
}