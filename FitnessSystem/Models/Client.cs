using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessSystem.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Пол обязателен")]
        [Display(Name = "Пол")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефон обязателен")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Неверный формат email")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Примечания")]
        public string? Notes { get; set; }

        [NotMapped]
        [Display(Name = "ФИО")]
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();

        [NotMapped]
        [Display(Name = "Возраст")]
        public int Age
        {
            get
            {
                var today = DateTime.UtcNow.Date;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
