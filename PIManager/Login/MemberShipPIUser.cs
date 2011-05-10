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

  private string comment;

  public string Comment
  {
    get { return comment; }
    set { comment = value; }
  }

  private DateTime creationDate;

  public DateTime CreationDate
  {
    get { return creationDate; }
    set { creationDate = value; }
  }

  private string email;

  public string Email
  {
    get { return email; }
    set { email = value; }
  }

  private bool isApproved;

  public bool IsApproved
  {
    get { return isApproved; }
    set { isApproved = value; }
  }

  private bool isLockedOut;

  public bool IsLockedOut
  {
    get { return isLockedOut; }
    set { isLockedOut = value; }
  }

  private bool isOnline;

  public bool IsOnline
  {
    get { return isOnline; }
    set { isOnline = value; }
  }

  private DateTime lastActivityDate;

  public DateTime LastActivityDate
  {
    get { return lastActivityDate; }
    set { lastActivityDate = value; }
  }

  private DateTime lastLockoutDate;

  public DateTime LastLockoutDate
  {
    get { return lastLockoutDate; }
    set { lastLockoutDate = value; }
  }

  private DateTime lastLoginDate;

  public DateTime LastLoginDate
  {
    get { return lastLoginDate; }
    set { lastLoginDate = value; }
  }

  private DateTime lastPasswordChangedDate;

  public DateTime LastPasswordChangedDate
  {
    get { return lastPasswordChangedDate; }
    set { lastPasswordChangedDate = value; }
  }

  private string passwordQuestion;

  public string PasswordQuestion
  {
    get { return passwordQuestion; }
    set { passwordQuestion = value; }
  }

  private string providerName;

  public string ProviderName
  {
    get { return providerName; }
    set { providerName = value; }
  }

  private object providerUserKey;

  public object ProviderUserKey
  {
    get { return providerUserKey; }
    set { providerUserKey = value; }
  }

  private string userName;

  public string UserName
  {
    get { return userName; }
    set { userName = value; }
  }

  private int pk_person;

  public int PK_Person
  {
      get { return pk_person; }
      set { pk_person = value; }
  }

    }
}