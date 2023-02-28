using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class PasswordResetModalMap : EntityTypeConfiguration<PasswordResetModal>
    {
        public PasswordResetModalMap()
        {
            this.HasKey(t => t.PasswordResetTableID);

            this.Property(t => t.UserID).IsRequired();
            this.Property(t => t.Code).HasMaxLength(20).IsRequired();
            this.Property(t => t.CreationDate).IsRequired();

            this.ToTable("PasswordResetTable");

            this.Property(t => t.PasswordResetTableID).HasColumnName("PasswordResetTableID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");

        }
    }
}
