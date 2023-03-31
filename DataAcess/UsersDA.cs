using DatabaseInterop;
using DatabaseInterop.Models;
using DTO;
using Emailer;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace DataAcess
{
    public class UsersDA : BaseDA
    {
        #region Private Random String Gen
        private string Generate6DigitCode()
        {
            return RandomChars("0123456789");
        }

        private static string RandomChars(string chars, int length = 6)
        {
            var randomString = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < length; i++)
                randomString.Append(chars[random.Next(chars.Length)]);

            return randomString.ToString();
        }
        #endregion Private Random String Gen

        EmailClient _emailer;

        public UsersDA(IUnitOfWork unitOfWork, IConfiguration _config): 
            base (unitOfWork) 
        {
            _emailer = new EmailClient(_config);
        }

        public async Task<bool> AddUser(UserModal user)
        {
            UnitOfWork.UserRepository.Insert(user);
            UnitOfWork.Save();

            EmailValidationModal email = new EmailValidationModal()
            {
                UserID = user.UserID,
                IsValid = false,
                ActivationCode = Generate6DigitCode(),
                ActivationCodeCreation = DateTime.Now
            };

            UnitOfWork.EmailValidationRepository.Insert(email);
            UnitOfWork.Save();

            await _emailer.SendEmailValidationEmail(user.Email, email.ActivationCode);

            return true;
        }

        public List<UserModal> GetUsers()
        {
            var users = (from Users in UnitOfWork.UserRepository.GetQuery()
                         select new UserModal()
                         {
                             Age = Users.Age,
                             Email = Users.Email,
                             UserName = Users.UserName,
                             UserID= Users.UserID,
                             PasswordHash= Users.PasswordHash,
                         }).ToList();

            return users;
        }

        public List<UserTransferModal> GetTransferModalUsers()
        {
            var userInfo = (from Users in UnitOfWork.UserRepository.GetQuery()
                            select Users);
            var users = (from info in UnitOfWork.EmailValidationRepository.GetQuery()
                         select info);

            return users.Join(userInfo, x => x.UserID, x => x.UserID, (userValid,userInfo) => new UserTransferModal()
            {
                Age = userInfo.Age,
                Email = userInfo.Email,
                UserName = userInfo.UserName,
                UserID = userInfo.UserID,
                IsValid = userValid.IsValid
            }).ToList();
        }

        public UserTransferModal GetUserTransferByUsername(string username)
        {
            var userInfo = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserName == username
                            select Users);

            var users = (from info in UnitOfWork.EmailValidationRepository.GetQuery()
                         where userInfo.Select(x => x.UserID == info.UserID).Count() > 0
                         select info);

            if (!userInfo.Any())
            {
                throw new Exception($"No users matching {username} exists");
            }

            if (!users.Any())
            {
                throw new Exception($"User with username {username} does not have an email validation entry");
            }

            return users.Join(userInfo, x => x.UserID, x => x.UserID, (userValid, userInfo) => new UserTransferModal()
            {
                Age = userInfo.Age,
                Email = userInfo.Email,
                UserName = userInfo.UserName,
                UserID = userInfo.UserID,
                IsValid = userValid.IsValid
            }).First();
        }

        public UserModal GetUserByUsername(string username)
        {
            try
            {
                var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserName == username
                            select Users).First();

                return user;
            }
            catch(Exception ex) {
                throw new Exception($"Unable to find user with the username: {username}");
            }

        }

        public UserModal GetUserByID(int id)
        {
            try
            {
                var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserID == id
                            select Users).First();

                return user;
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to find user with the id: {id}");
            }
        }

        public bool ValidateEmail(string code)
        {
            try
            {
                var emailValidation = (from Validation in UnitOfWork.EmailValidationRepository.GetQuery()
                                       where Validation.ActivationCode == code
                                       select Validation).First();

                if (emailValidation.ActivationCodeCreation?.AddMinutes(30) > DateTime.Now)
                {
                    if (emailValidation.ActivationCode == code)
                    {
                        emailValidation.IsValid = true;
                        emailValidation.ActivationCodeCreation = null;
                        emailValidation.ActivationCode = null;
                        UnitOfWork.EmailValidationRepository.Update(emailValidation);
                        UnitOfWork.Save();
                        return true;
                    }
                    return false;
                }
                UnitOfWork.EmailValidationRepository.Delete(emailValidation);
                UnitOfWork.Save();
                throw new Exception("Code has expired Generate a new code before proceeding");
            }
            catch(Exception ex)
            {
                throw new Exception($"No emailvalidation found with the code {code}");
            }
        }

        public async Task<bool> RequestNewEmailValidationCode(string username) {
            try
            {
                var user = (from Users in UnitOfWork.UserRepository.GetQuery()
                            where Users.UserName == username
                            select Users).First();
                try
                {
                    var emailValidation = (from Validation in UnitOfWork.EmailValidationRepository.GetQuery()
                                           where Validation.UserID == user.UserID
                                           select Validation).First();

                    if (emailValidation.IsValid)
                    {
                        throw new Exception($"The user with the username {user.UserName} has already been validated");
                    }


                    emailValidation.ActivationCodeCreation = DateTime.Now;
                    emailValidation.IsValid = false;
                    emailValidation.ActivationCode = Generate6DigitCode();
                    UnitOfWork.EmailValidationRepository.Update(emailValidation);
                    UnitOfWork.Save();


                    await _emailer.SendEmailValidationEmail(user.Email, emailValidation.ActivationCode);
                }
                catch(Exception ex)
                {
                    var emailValidation = new EmailValidationModal();
                    emailValidation.ActivationCodeCreation = DateTime.Now;
                    emailValidation.IsValid = false;
                    emailValidation.ActivationCode = Generate6DigitCode();
                    emailValidation.UserID = user.UserID;

                    UnitOfWork.EmailValidationRepository.Insert(emailValidation);
                    UnitOfWork.Save();
                    
                    await _emailer.SendEmailValidationEmail(user.Email, emailValidation.ActivationCode);
                }
                return true;
            }
            catch(Exception ex)
            {
                if(ex.Message == $"The user with the username {username} has already been validated")
                {
                    throw ex;
                }
                if(ex.Message == "Sequence contains no elements")
                {
                    throw new Exception($"No user with the username {username} exists");
                }
                throw new Exception($"Unable to find an email validation for the username: {username}");
            }
        }

        public bool AuthorizeUser(string username, string password)
        {
            var user = GetUserByUsername(username);

            var emailCheck = (from Validation in UnitOfWork.EmailValidationRepository.GetQuery()
                              where Validation.UserID == user.UserID
                              select Validation).First();
            if (emailCheck.ActivationCode == null)
            {
                return user.CheckPasswordHash(password);
            }
            throw new Exception($"This account with the userID = {user.UserID} does not have a valid email");
        }

        public async Task<bool> RequestNewPasswordCode(string email)
        {
            var user = (from User in UnitOfWork.UserRepository.GetQuery()
                        where User.Email == email
                        select User).FirstOrDefault();


            if (user == null)
            {
                throw new Exception($"Unable to find user with the email: {email}");
            }

            var pass_reset = new PasswordResetModal()
            {
                Code = RandomChars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 20),
                CreationDate = DateTime.Now,
                UserID = user.UserID
            };

            UnitOfWork.PasswordResetRepository.Insert(pass_reset);
            UnitOfWork.Save();

            await _emailer.SendPasswordResetEmail(user.Email, pass_reset.Code);

            return true;
        }

        public bool ResetPassword(string code, string new_pass)
        {
            var pass_reset = (from PassReset in UnitOfWork.PasswordResetRepository.GetQuery()
                                  where code == PassReset.Code
                                  select PassReset).FirstOrDefault();

            if(pass_reset == null)
            {
                throw new Exception($"No code matching {code} has been found");
            }

            if (pass_reset != null && pass_reset.CreationDate.AddMinutes(60) < DateTime.Now)
            {
                UnitOfWork.PasswordResetRepository.Delete(pass_reset);
                UnitOfWork.Save();
                throw new Exception($"The code {code}\nhas expired, please request a new code");
            }

            var user = GetUserByID(pass_reset.UserID);

            UnitOfWork.PasswordResetRepository.Delete(pass_reset);

            user.CreatePasswordHash(new_pass);

            UnitOfWork.UserRepository.Update(user);
            UnitOfWork.Save();
            return true;
        }

        public string GetApiCodeForUser(int userID)
        {
            try
            {
                var user = GetUserByID(userID);

                if(user == null)
                {
                    throw new Exception($"Unable to find user with the id {userID}");
                }

                try
                {
                    var others = (from ApiAuth in UnitOfWork.ApiAuthCodeRepository.GetQuery()
                                  where ApiAuth.UserID == userID
                                  select ApiAuth).First();

                    if (others != null)
                    {
                        if (others.CreationDate.AddDays(7) > DateTime.Now)
                        {
                            return others.Code;
                        }

                        others.Code = RandomChars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 30);
                        UnitOfWork.ApiAuthCodeRepository.Update(others);
                        UnitOfWork.Save();
                        return others.Code;
                    }
                }
                catch (Exception ex)
                {

                }
                
                var codeModel = new ApiAuthCodeTableModel()
                {
                    CreationDate = DateTime.Now,
                    UserID = user.UserID,
                    Code = RandomChars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890", 30)
                };

                UnitOfWork.ApiAuthCodeRepository.Insert(codeModel);
                UnitOfWork.Save();
                return codeModel.Code;
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public UserTransferModal GetUserByApiCode(string code)
        {
            var codeModel = (from ApiAuthCode in UnitOfWork.ApiAuthCodeRepository.GetQuery()
                             where ApiAuthCode.Code == code
                             select ApiAuthCode).FirstOrDefault();
            if (codeModel == null)
            {
                throw new Exception($"Unable to find the code {code}");
            }
            
            if (codeModel.CreationDate.AddDays(7) < DateTime.Now)
            {
                UnitOfWork.ApiAuthCodeRepository.Delete(codeModel);
                UnitOfWork.Save();
                throw new Exception($"The code {code} has expired");
            }

            var user = GetUserByID(codeModel.UserID);
            return GetUserTransferByUsername(user.UserName);
        }

        public bool CheckIfValid(string username, string email)
        {
            return (from Users in UnitOfWork.UserRepository.GetQuery()
                    where Users.UserName == username || Users.Email == email
                    select Users).Count() == 0;
        }

        public async Task<bool> TestMailer(string to, string subject, string body)
        {
            try
            {
                await _emailer.SendEmail(to, subject, body);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public EmailClient GetEmailClient() { 
            return _emailer; 
        }

        public bool CheckIfUserEmailValid(string username)
        {
            var user = GetUserTransferByUsername(username);
            return user.IsValid;
        }

        public async Task<bool> TestPassResetEmail(string to)
        {
            try
            {
                await _emailer.SendTaskReminderEmail(to, new TaskModal()
                {
                    Description = "<script>alert(1)</script>",
                    Title = "Test",
                    TaskID = 1
                });
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
