using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PIManager.Login
{
    /// <summary>
    /// Represent a connected user
    /// </summary>
    public class MemberShipPIUser : MembershipUser
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="pkPerson">Primary key of the person connected</param>
        /// <param name="userName">User name of the person connected</param>
        /// <param name="isOnline">true if person is online, false otherwise</param>
        public MemberShipPIUser(int pkPerson, string userName, bool isOnline)
        {
            PkPerson = pkPerson;
            UserName = userName;
            IsOnline = isOnline;
        }

        /*protected string myComment;

        public override string Comment
        {
            get { return myComment; }
            set { myComment = value; }
        }

        protected DateTime myCreationDate;

        public override DateTime CreationDate
        {
            get { return myCreationDate; }
            //set { myCreationDate = value; }
        }

        protected string myEmail;

        public override string Email
        {
            get { return myEmail; }
            set { myEmail = value; }
        }

        protected bool myIsApproved;

        public override bool IsApproved
        {
            get { return myIsApproved; }
            set { myIsApproved = value; }
        }

        protected bool myIsLockedOut;

        public override bool IsLockedOut
        {
            get { return myIsLockedOut; }
            set { myIsLockedOut = value; }
        }*/

        /// <summary>
        /// true if the person is online, false otherwise
        /// </summary>
        protected bool myIsOnline;

        /// <summary>
        /// true if the person is online, false otherwise
        /// </summary>
        public new bool IsOnline
        {
            get { return myIsOnline; }
            set { myIsOnline = value; }
        }

        /*protected DateTime myLastActivityDate;

        public override DateTime LastActivityDate
        {
            get { return myLastActivityDate; }
            set { myLastActivityDate = value; }
        }

        protected DateTime myLastLockoutDate;

        public override DateTime LastLockoutDate
        {
            get { return myLastLockoutDate; }
            set { myLastLockoutDate = value; }
        }

        protected DateTime myLastLoginDate;

        public override DateTime LastLoginDate
        {
            get { return myLastLoginDate; }
            set { myLastLoginDate = value; }
        }

        protected DateTime myLastPasswordChangedDate;

        public override DateTime LastPasswordChangedDate
        {
            get { return myLastPasswordChangedDate; }
            set { myLastPasswordChangedDate = value; }
        }

        protected string myPasswordQuestion;

        public override string PasswordQuestion
        {
            get { return myPasswordQuestion; }
            set { myPasswordQuestion = value; }
        }

        protected string myProviderName;

        public override string ProviderName
        {
            get { return myProviderName; }
            set { myProviderName = value; }
        }

        protected object myProviderUserKey;

        public override object ProviderUserKey
        {
            get { return myProviderUserKey; }
            set { myProviderUserKey = value; }
        }*/

        /// <summary>
        /// User name of the person
        /// </summary>
        protected string myUserName;

        /// <summary>
        /// User name of the person
        /// </summary>
        public new string UserName
        {
            get { return myUserName; }
            set { myUserName = value; }
        }

        /// <summary>
        /// Primary key of the person
        /// </summary>
        protected int myPkPerson;

        /// <summary>
        /// Primary key of the person
        /// </summary>
        public int PkPerson
        {
            get { return myPkPerson; }
            set { myPkPerson = value; }
        }
    }
}