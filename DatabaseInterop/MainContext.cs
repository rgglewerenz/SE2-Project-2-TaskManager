﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DatabaseInterop.Models.Mapping;
using DatabaseInterop.Models;
using Microsoft.Extensions.Configuration;

namespace DatabaseInterop
{
    public class MainContext: DbContext
    {
        private static string GetConnectionString(IConfiguration _config)
        {
            return _config.GetSection("ConnectionStrings").GetSection("Main").Value ?? "";
        }

        public MainContext(IConfiguration _config) : base(GetConnectionString(_config)) {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserModalMap()); 
            modelBuilder.Configurations.Add(new TaskModalMap());
            modelBuilder.Configurations.Add(new UserTaskMapMap());
            modelBuilder.Configurations.Add(new EmailValidationMap());
            modelBuilder.Configurations.Add(new TaskRecurrenceModalMap());
            modelBuilder.Configurations.Add(new PasswordResetModalMap());
            modelBuilder.Configurations.Add(new ApiAuthCodeTableMap());
        }



    }
}
