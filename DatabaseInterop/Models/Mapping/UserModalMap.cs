using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class UserModalMap: EntityTypeConfiguration<UserModal>
    {

        public UserModalMap()
        {

            this.HasKey(t => t.UserID);

            this.Property(t => t.UserName).IsRequired().HasMaxLength(150);
            this.Property(t => t.Email).IsRequired().HasMaxLength(150);
            this.Property(t => t.Age).IsRequired();
            this.Property(t => t.PasswordHash).IsRequired().HasMaxLength(150);

            this.ToTable("Users");

            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Age).HasColumnName("Age");
            this.Property(t => t.PasswordHash).HasColumnName("PasswordHash");

        }
    }
}
