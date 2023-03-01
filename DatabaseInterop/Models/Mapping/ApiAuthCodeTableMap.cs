using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models.Mapping
{
    public class ApiAuthCodeTableMap: EntityTypeConfiguration<ApiAuthCodeTableModel>
    {
        public ApiAuthCodeTableMap()
        {

            this.HasKey(t => t.ApiAuthCodeTableID);

            this.Property(t => t.UserID).IsRequired();
            this.Property(t => t.Code).HasMaxLength(30).IsRequired();
            this.Property(t => t.CreationDate).IsRequired();

            this.ToTable("ApiAuthCodeTable");
            this.Property(t => t.ApiAuthCodeTableID).HasColumnName("ApiAuthCodeTableID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");

        }
    }
}
