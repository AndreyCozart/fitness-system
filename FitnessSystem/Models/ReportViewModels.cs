using System;
using System.Collections.Generic;

namespace FitnessSystem.Models
{
    public class VisitsReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalVisits { get; set; }
        public int UniqueClients { get; set; }
        public List<DailyVisits> DailyVisits { get; set; } = new();
        public List<HourlyLoad> HourlyLoad { get; set; } = new();
    }

    public class DailyVisits
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class HourlyLoad
    {
        public int Hour { get; set; }
        public int AverageVisits { get; set; }
    }

    public class RevenueReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, decimal> RevenueByMembershipType { get; set; } = new();
    }

    public class GroupClassesReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<string, int> PopularityByClass { get; set; } = new();
        public Dictionary<string, int> PopularityByInstructor { get; set; } = new();
    }
}