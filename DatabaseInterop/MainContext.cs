using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DatabaseInterop.Models.Mapping;
using DatabaseInterop.Models;

namespace DatabaseInterop
{
    public class MainContext: DbContext
    {
        private static string ConnectionString = "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

        

        public MainContext() : base(ConnectionString){
        
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserModalMap()); 
            modelBuilder.Configurations.Add(new TaskModalMap());
            modelBuilder.Configurations.Add(new UserTaskMapMap());
            modelBuilder.Configurations.Add(new EmailValidationMap());
            modelBuilder.Configurations.Add(new TaskRecurrenceModalMap());
            modelBuilder.Configurations.Add(new PasswordResetModalMap());
        }
    }
}
