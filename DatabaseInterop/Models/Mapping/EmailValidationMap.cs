using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class EmailValidationMap : EntityTypeConfiguration<EmailValidationModal>
    {

        public EmailValidationMap() {

            this.HasKey(t => t.EmailValidationID);

            this.Property(t => t.UserID).IsRequired();
            this.Property(t => t.IsValid).IsRequired();
            this.Property(t => t.ActivationCodeCreation);
            this.Property(t => t.ActivationCode).HasMaxLength(6);

            this.ToTable("EmailValidationTable");
            this.Property(t => t.EmailValidationID).HasColumnName("EmailValidationID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.IsValid).HasColumnName("IsValid");
            this.Property(t => t.ActivationCode).HasColumnName("ActivationCode");
            this.Property(t => t.ActivationCodeCreation).HasColumnName("ActivationCodeCreation");

        }
    }
}
