using System;
using System.Collections.Generic;

namespace FitnessSystem.Models
{
    public class ScheduleDayViewModel
    {
        public DateTime Date { get; set; }
        public string DayName { get; set; } = string.Empty;
        public List<ScheduleClassViewModel> Classes { get; set; } = new List<ScheduleClassViewModel>();
    }

    public class ScheduleClassViewModel
    {
        public GroupClass Class { get; set; } = new GroupClass();
        public int BookingsCount { get; set; }
    }
}