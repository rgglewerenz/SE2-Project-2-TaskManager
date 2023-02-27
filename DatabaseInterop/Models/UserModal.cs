﻿using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DTO;

namespace DatabaseInterop.Models
{
    public class UserModal
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public int Age { get; set; }
        public int UserID { get; set; }
        public string PasswordHash { get; set; } = "";

        public void CreatePasswordHash(string basePassword)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(Encoding.ASCII.GetBytes(basePassword), salt, 100000);

            byte[] hashBytes = new byte[36];
            byte[] hash = pbkdf2.GetBytes(20);
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            PasswordHash = Convert.ToBase64String(hashBytes);
        }

        public bool CheckPasswordHash(string testPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(PasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(testPassword, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }


        //Used for converting UserModals into UserTransferModals
        public static explicit operator UserTransferModal(UserModal user)
        {
            return new UserTransferModal()
            {
                UserID = user.UserID,
                UserName = user.UserName,
                Age = user.Age,
                Email = user.Email,
            };
        }
    }
}
