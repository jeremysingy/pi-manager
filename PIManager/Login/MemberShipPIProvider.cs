using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using PIManager.DAO;
using System.Data.SqlClient;
using System.Data;

namespace PIManager.Login
{
    /// <summary>
    /// Used to handle registrations of users 
    /// </summary>
    public class MemberShipPIProvider : MembershipProvider
    {
        private MemberShipPIUser user;

        /// <summary>
        /// Name of the application
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return "Application Name";
            }
            set
            {
                
            }
            
        }

        /// <summary>
        /// Change password (not implemented)
        /// </summary>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            //throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// Change password (not implemented)
        /// </summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            //throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// Create password (not implemented)
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            //throw new NotImplementedException();
            status = new MembershipCreateStatus();
            return null;
        }

        /// <summary>
        /// Delete user (not implemented)
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            //throw new NotImplementedException();
            return true;
        }

        /// <summary>
        /// Enable password reset (not implemented)
        /// </summary>
        public override bool EnablePasswordReset
        {
            get
            { //throw new NotImplementedException();
                return true;
            }
        }

        /// <summary>
        /// Enable password retrieval (not implemented)
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Find users (not implemented)
        /// </summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary>
        /// Find users (not implemented)
        /// </summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary>
        /// Get all users (not implemented)
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return null;
        }

        /// <summary>
        /// Get online users (not implemented)
        /// </summary>
        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        /// <summary>
        /// Get password (not implemented)
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            return "no pass";
        }

        /// <summary>
        /// Get user (not implemented)
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return user;
        }

        /// <summary>
        /// Get user (not implemented)
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return null;
        }

        /// <summary>
        /// Get user name (not implemented)
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            //throw new NotImplementedException();
            return "no username";
        }

        /// <summary>
        /// Get maximum invalid password attempts (not implemented)
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Get minimum invalid password attempts (not implemented)
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Get minimum required password length (not implemented)
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Get passsword attempt window (not implemented)
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Get the password format (not implemented)
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return new MembershipPasswordFormat();
            }
        }

        /// <summary>
        /// Get the password strength regular expession (not implemented)
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return "---";
            }
        }

        /// <summary>
        /// Get if requires question/answer (not implemented)
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Get if requires unique email (not implemented)
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Reset password (not implemented)
        /// </summary>
        public override string ResetPassword(string username, string answer)
        {
            return "---";
        }

        /// <summary>
        /// Unlock user (not implemented)
        /// </summary>
        public override bool UnlockUser(string userName)
        {
            return true;
        }

        /// <summary>
        /// Update user (not implemented)
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            
        }

        /// <summary>
        /// Validate login of a user
        /// <param name="username">User Name of the person</param>
        /// <param name="password">Password of the person</param>
        /// </summary>
        public override bool ValidateUser(string username, string password)
        {
            bool ok = false;

            DBManager dbmanager = new DBManager();

            int pk_person = -1;

            string query = "SELECT pk_person FROM Person WHERE Person.login = @login AND Person.password = HASHBYTES('MD5', @pass)";

            SqlConnection connection = dbmanager.newConnection();
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "Login");

            List<SqlParameter> param = new List<SqlParameter>();
            SqlParameter loginParam = new SqlParameter("@login", username);
            loginParam.SqlDbType = SqlDbType.VarChar;
            SqlParameter passParam = new SqlParameter("@pass", password);
            passParam.SqlDbType = SqlDbType.VarChar;

            param.Add(loginParam);
            param.Add(passParam);

            object o = dbmanager.doSelectScalar(query, connection, transaction, param);

            connection.Close();

            if (o != null && !DBNull.Value.Equals(o))
            {
                pk_person = (int)o;
            }

            if (pk_person != -1)
            {
                ok = true;

                int roleID = -1;

                string query_role = "SELECT role FROM Person WHERE pk_person = @pk_person;";

                SqlConnection connection_role = dbmanager.newConnection();
                connection_role.Open();

                SqlTransaction transaction_role = connection_role.BeginTransaction(IsolationLevel.ReadCommitted, "getTypePerson");

                Dictionary<string, object> param_role = new Dictionary<string, object>();
                param_role.Add("@pk_person", pk_person);

                object o_role = dbmanager.doSelectScalar(query_role, connection_role, transaction_role, param_role);

                connection_role.Close();

                if (!DBNull.Value.Equals(o_role) && o_role != null)
                {
                    roleID = (int)o_role;
                }
                else
                {
                    ok = false;
                }

                // add user in role if not in
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

                user = new MemberShipPIUser(pk_person, username, true);
            }

            return ok;
        }
    }
}