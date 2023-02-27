using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class UserTaskMapMap : EntityTypeConfiguration<UserTaskMap>
    {
        public UserTaskMapMap() {
            this.HasKey(t => t.UserTaskMapID);

            this.Property(t => t.UserID).IsRequired();
            this.Property(t => t.TaskID).IsRequired();

            this.ToTable("UserTasksMap");
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserTaskMapID).HasColumnName("UserTaskMapID");


        }
    }
}
