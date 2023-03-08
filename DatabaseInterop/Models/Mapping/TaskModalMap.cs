using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class TaskModalMap : EntityTypeConfiguration<TaskModal>
    {
        public TaskModalMap() {
            this.HasKey(t => t.TaskID);

            this.Property(t => t.Title).IsRequired().HasMaxLength(50);
            this.Property(t => t.Description).IsRequired().HasMaxLength(500);

            this.ToTable("Tasks");
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");

        }
    }
}
