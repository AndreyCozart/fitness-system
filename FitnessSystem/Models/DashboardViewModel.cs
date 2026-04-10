using System;
using System.Collections.Generic;

namespace FitnessSystem.Models
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }
        public int ActiveMemberships { get; set; }
        public int TodayVisits { get; set; }
        public int ClientsInGym { get; set; }

        public List<RecentClient> RecentClients { get; set; } = new List<RecentClient>();
        public List<ExpiringMembership> ExpiringMemberships { get; set; } = new List<ExpiringMembership>();
        public List<VisitStatistics> VisitsByDay { get; set; } = new List<VisitStatistics>();
        public List<PopularMembershipType> PopularMembershipTypes { get; set; } = new List<PopularMembershipType>();
    }

    public class RecentClient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
    }

    public class ExpiringMembership
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string MembershipType { get; set; } = string.Empty;
        public DateTime EndDate { get; set; }
        public int DaysLeft { get; set; }
    }

    public class VisitStatistics
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class PopularMembershipType
    {
        public string TypeName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}