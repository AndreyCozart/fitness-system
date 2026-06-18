using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessSystem.Models
{
    public class Membership
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Тип абонемента")]
        public int TypeId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Осталось посещений")]
        public int VisitsRemaining { get; set; }

        [Display(Name = "Статус")]
        public string Status { get; set; } = "active";

        [Display(Name = "Дней заморозки")]
        public int FreezeDays { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Дата покупки")]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        [ForeignKey("TypeId")]
        public virtual MembershipType? Type { get; set; }

        public virtual ICollection<Visit> Visits { get; set; } = new List<Visit>();

        [NotMapped]
        public bool IsActive => Status == "active"
            && EndDate >= DateTime.UtcNow.Date
            && (Type?.VisitsCount == null || VisitsRemaining > 0);

        [NotMapped]
        public bool IsExpired => Status == "expired" || EndDate < DateTime.UtcNow.Date;

        [NotMapped]
        public bool IsFrozen => Status == "frozen";

        [NotMapped]
        [Display(Name = "Дней до окончания")]
        public int DaysUntilExpiry => (EndDate - DateTime.UtcNow.Date).Days;
    }
}
