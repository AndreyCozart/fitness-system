using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessSystem.Models
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Тип абонемента")]
        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public virtual MembershipType Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Осталось посещений")]
        public int VisitsRemaining { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Статус")]
        public string Status { get; set; } = "active"; // active, expired, frozen

        [Display(Name = "Дней заморозки")]
        public int FreezeDays { get; set; } = 0;

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата покупки")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        // Навигационное свойство
        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

        [NotMapped]
        public bool IsActive => Status == "active" && EndDate >= DateTime.Today && (Type.VisitsCount == null || VisitsRemaining > 0);

        [NotMapped]
        public bool IsExpired => Status == "expired" || EndDate < DateTime.Today;

        [NotMapped]
        public bool IsFrozen => Status == "frozen";

        [NotMapped]
        [Display(Name = "Дней до окончания")]
        public int DaysUntilExpiry => (EndDate - DateTime.Today).Days;
    }
}