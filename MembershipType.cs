using System.ComponentModel.DataAnnotations;

namespace FitnessSystem.Models
{
    public class MembershipType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа абонемента обязательно")]
        [StringLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
        [Display(Name = "Название")]
        public string TypeName { get; set; }

        [Display(Name = "Количество посещений")]
        public int? VisitsCount { get; set; } // null для безлимитных

        [Required(ErrorMessage = "Срок действия обязателен")]
        [Range(1, 3650, ErrorMessage = "Срок действия должен быть от 1 до 3650 дней")]
        [Display(Name = "Срок действия (дней)")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Стоимость обязательна")]
        [Range(0, 1000000, ErrorMessage = "Стоимость должна быть от 0 до 1 000 000")]
        [DataType(DataType.Currency)]
        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }

        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Описание не может быть длиннее 500 символов")]
        public string? Description { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        // Навигационное свойство
        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

        [NotMapped]
        [Display(Name = "Тип")]
        public string DisplayName => $"{TypeName} - {Price:N0} ₽";

        [NotMapped]
        public bool IsUnlimited => !VisitsCount.HasValue;
    }
}