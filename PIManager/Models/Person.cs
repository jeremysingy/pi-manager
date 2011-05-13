using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.Models
{
    /// <summary>
    /// Represent a person
    /// </summary>
    public class Person
    {
        private int myId;
        private string myLastName;
        private string myFirstName;
        private string myEmail;
        private string myLogin;
        private int myRole;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the person</param>
        /// <param name="lastName">Last name of the person</param>
        /// <param name="firstName">First name of the person</param>
        /// <param name="email">Email of the person</param>
        /// <param name="login">Login of the person</param>
        /// <param name="role">Role of the person (student or professor)</param>
        public Person(int id, string lastName, string firstName, string email, string login, int role)
        {
            myId = id;
            myLastName = lastName;
            myFirstName = firstName;
            myEmail = email;
            myLogin = login;
            myRole = role;
        }

        public int Id
        {
            get { return myId; }
            set { myId = value; }
        }

        public string LastName
        {
            get { return myLastName; }
            set { myLastName = value; }
        }

        public string FirstName
        {
            get { return myFirstName; }
            set { myFirstName = value; }
        }

        public string CompleteName
        {
            get { return myFirstName + " " + myLastName; }
        }

        public string Email
        {
            get { return myEmail; }
            set { myEmail = value; }
        }

        public string Login
        {
            get { return myLogin; }
            set { myLogin = value; }
        }

        public int Role
        {
            get { return myRole; }
            set { myRole = value; }
        }
    }
}