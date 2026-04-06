using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessSystem.Models
{
    public class MembershipType
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string TypeName { get; set; } = string.Empty;

        [Display(Name = "Количество посещений")]
        public int? VisitsCount { get; set; }

        [Display(Name = "Срок действия (дней)")]
        public int DurationDays { get; set; }

        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        // Навигационное свойство
        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}