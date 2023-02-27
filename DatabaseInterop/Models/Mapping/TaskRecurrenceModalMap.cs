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
            this.Property(t => t.RecurringDays).HasMaxLength(31);
            this.Property(t => t.RecurringType).IsRequired();
            this.Property(t => t.FirstOccurrance).IsRequired();

            this.ToTable("TaskRecurranceMap");


            this.Property(t => t.TaskRecurrenceMapID).HasColumnName("TaskRecurrenceMapID");
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            this.Property(t => t.RecurringType).HasColumnName("RecurringType");
            this.Property(t => t.RecurringDays).HasColumnName("RecurringDays");
            this.Property(t => t.FirstOccurrance).HasColumnName("FirstOccurrance");


        }
    }
}
