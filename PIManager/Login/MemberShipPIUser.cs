using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PIManager.Login
{
    public class MemberShipPIUser : MembershipUser
    {
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

        protected bool myIsOnline;

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

        protected string myUserName;

        public new string UserName
        {
            get { return myUserName; }
            set { myUserName = value; }
        }

        protected int myPkPerson;

        public int PkPerson
        {
            get { return myPkPerson; }
            set { myPkPerson = value; }
        }
    }
}