using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PIManager.Login
{
    public class MemberShipPIUser : MembershipUser
    {
        public MemberShipPIUser()
        {
        }

        private string myComment;

        public string Comment
        {
            get { return myComment; }
            set { myComment = value; }
        }

        private DateTime myCreationDate;

        public DateTime CreationDate
        {
            get { return myCreationDate; }
            set { myCreationDate = value; }
        }

        private string myEmail;

        public string Email
        {
            get { return myEmail; }
            set { myEmail = value; }
        }

        private bool myIsApproved;

        public bool IsApproved
        {
            get { return myIsApproved; }
            set { myIsApproved = value; }
        }

        private bool myIsLockedOut;

        public bool IsLockedOut
        {
            get { return myIsLockedOut; }
            set { myIsLockedOut = value; }
        }

        private bool myIsOnline;

        public bool IsOnline
        {
            get { return myIsOnline; }
            set { myIsOnline = value; }
        }

        private DateTime myLastActivityDate;

        public DateTime LastActivityDate
        {
            get { return myLastActivityDate; }
            set { myLastActivityDate = value; }
        }

        private DateTime myLastLockoutDate;

        public DateTime LastLockoutDate
        {
            get { return myLastLockoutDate; }
            set { myLastLockoutDate = value; }
        }

        private DateTime myLastLoginDate;

        public DateTime LastLoginDate
        {
            get { return myLastLoginDate; }
            set { myLastLoginDate = value; }
        }

        private DateTime myLastPasswordChangedDate;

        public DateTime LastPasswordChangedDate
        {
            get { return myLastPasswordChangedDate; }
            set { myLastPasswordChangedDate = value; }
        }

        private string myPasswordQuestion;

        public string PasswordQuestion
        {
            get { return myPasswordQuestion; }
            set { myPasswordQuestion = value; }
        }

        private string myProviderName;

        public string ProviderName
        {
            get { return myProviderName; }
            set { myProviderName = value; }
        }

        private object myProviderUserKey;

        public object ProviderUserKey
        {
            get { return myProviderUserKey; }
            set { myProviderUserKey = value; }
        }

        private string myUserName;

        public string UserName
        {
            get { return myUserName; }
            set { myUserName = value; }
        }

        private int myPkPerson;

        public int PkPerson
        {
            get { return myPkPerson; }
            set { myPkPerson = value; }
        }
    }
}