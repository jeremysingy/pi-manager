﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PIManager.DataAccess;

namespace PIManager.Login
{
    public class MemberShipPIProvider : MembershipProvider
    {
        private MemberShipPIUser user;

        public override string ApplicationName
        {
            get
            {
                //throw new NotImplementedException();
                return "Application Name";
            }
            set
            {
                //throw new NotImplementedException();
            }
            
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            //throw new NotImplementedException();
            status = new MembershipCreateStatus();
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override bool EnablePasswordReset
        {
            get
            { //throw new NotImplementedException();
                return true;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            { //throw new NotImplementedException();
                return true;
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //throw new NotImplementedException();
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //throw new NotImplementedException();
            totalRecords = 0;
            return null;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //throw new NotImplementedException();
            totalRecords = 0;
            return null;
        }

        public override int GetNumberOfUsersOnline()
        {
            //throw new NotImplementedException();
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            //throw new NotImplementedException();
            return "no pass";
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return user;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        public override string GetUserNameByEmail(string email)
        {
            //throw new NotImplementedException();
            return "no username";
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            { //throw new NotImplementedException();
                return 0;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            { //throw new NotImplementedException();
                return 0;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            { //throw new NotImplementedException();
                return 0;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            { //throw new NotImplementedException();
                return 0;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return new MembershipPasswordFormat();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            { //throw new NotImplementedException();
                return "---";
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            { //throw new NotImplementedException();
                return true;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            { //throw new NotImplementedException();
                return false;
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            //throw new NotImplementedException();
            return "---";
        }

        public override bool UnlockUser(string userName)
        {
            //throw new NotImplementedException();
            return true;
        }

        public override void UpdateUser(MembershipUser user)
        {
            //throw new NotImplementedException();
            
        }

        public override bool ValidateUser(string username, string password)
        {
            bool ok = false;

            DBManager dbmanager = new DBManager();

            int pk_person = dbmanager.getPerson(username, password);

            if (pk_person != -1)
            {
                ok = true;

                int roleID = -1;

                roleID = dbmanager.getPersonType(pk_person);

                    

                //add user in role if not in
                if (roleID == 1)
                {
                    if (!Roles.RoleExists("student"))
                    {
                        Roles.CreateRole("student");
                    }
                    if(!Roles.IsUserInRole(username, "student"))
                    {
                        Roles.AddUserToRole(username, "student");
                    }

                    
                }
                else
                {
                    if (!Roles.RoleExists("professor"))
                    {
                        Roles.CreateRole("professor");
                    }
                    if (!Roles.IsUserInRole(username, "professor"))
                    {
                        Roles.AddUserToRole(username, "professor");
                    }
                    
                }

                user = new MemberShipPIUser();
                user.PK_Person = pk_person;
                user.UserName = username;
                user.IsOnline = true;
                
            }

            return ok;
        }
    }
}