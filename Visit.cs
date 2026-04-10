using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessSystem.Models
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Клиент")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [Required]
        [Display(Name = "Абонемент")]
        public int MembershipId { get; set; }

        [ForeignKey("MembershipId")]
        public virtual Membership Membership { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Время прихода")]
        public DateTime CheckInTime { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Время ухода")]
        public DateTime? CheckOutTime { get; set; }

        [StringLength(500)]
        [Display(Name = "Примечания")]
        public string? Notes { get; set; }

        [NotMapped]
        public TimeSpan? Duration => CheckOutTime.HasValue ? CheckOutTime - CheckInTime : null;

        [NotMapped]
        public bool IsActive => !CheckOutTime.HasValue;
    }
}