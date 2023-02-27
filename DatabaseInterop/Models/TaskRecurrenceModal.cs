using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models
{
    public class TaskRecurrenceModal
    {
        public int TaskRecurrenceMapID { get; set; }
        public int TaskID { get; set; }
        public bool IsRecurring { get; set; }
        public bool IsRecurringWeekly { get; set; }
        public bool IsRecurringBiWeekly { get; set; }
        public bool IsRecurringMonthly { get; set; }
        public string? MonthlyRecurringDays { get; set; }
        public string? WeeklyRecurringDays { get; set; }
        public DateTime? FirstOccurrance { get; set; }
    }
}
