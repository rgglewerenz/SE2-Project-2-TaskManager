using DatabaseEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models
{
    public class TaskRecurrenceModal
    {
        public int TaskRecurranceMapID { get; set; }
        public int TaskID { get; set; }
        public DatabaseEnums.RecurrentTypes RecurringType { get; set; } = RecurrentTypes.Never;
        public string? RecurringDays { get; set; }
        public DateTime? FirstOccurrance { get; set; }
    }
}
