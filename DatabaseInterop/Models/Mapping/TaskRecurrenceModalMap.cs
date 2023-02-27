using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class TaskRecurrenceModalMap : EntityTypeConfiguration<TaskRecurrenceModal>
    {
        public TaskRecurrenceModalMap() {
            this.HasKey(t => t.TaskRecurrenceMapID);

            this.Property(t => t.TaskID).IsRequired();
            this.Property(t => t.WeeklyRecurringDays).HasMaxLength(7);
            this.Property(t => t.MonthlyRecurringDays).HasMaxLength(31);
            this.Property(t => t.IsRecurring).IsRequired();
            this.Property(t => t.IsRecurringBiWeekly).IsRequired();
            this.Property(t => t.IsRecurringMonthly).IsRequired();
            this.Property(t => t.IsRecurringWeekly).IsRequired();
            this.Property(t => t.FirstOccurrance).IsRequired();

            this.ToTable("TaskRecurranceMap");


            this.Property(t => t.TaskRecurrenceMapID).HasColumnName("TaskRecurrenceMapID");
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            this.Property(t => t.WeeklyRecurringDays).HasColumnName("WeeklyRecurringDays");
            this.Property(t => t.MonthlyRecurringDays).HasColumnName("MonthlyRecurringDays");
            this.Property(t => t.IsRecurring).HasColumnName("IsRecurring");
            this.Property(t => t.IsRecurringBiWeekly).HasColumnName("IsRecurringBiWeekly");
            this.Property(t => t.IsRecurringMonthly).HasColumnName("IsRecurringMonthly");
            this.Property(t => t.IsRecurringWeekly).HasColumnName("IsRecurringWeekly");
            this.Property(t => t.FirstOccurrance).HasColumnName("FirstOccurrance");


        }
    }
}
