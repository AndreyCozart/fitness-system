using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessSystem.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int MembershipId { get; set; }
        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOutTime { get; set; }
        public string? Notes { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        [ForeignKey("MembershipId")]
        public virtual Membership? Membership { get; set; }
    }
}
