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

        /// <summary>
        /// Id of the person
        /// </summary>
        public int Id
        {
            get { return myId; }
            set { myId = value; }
        }

        /// <summary>
        /// Last name of the person
        /// </summary>
        public string LastName
        {
            get { return myLastName; }
            set { myLastName = value; }
        }

        /// <summary>
        /// First name of the person
        /// </summary>
        public string FirstName
        {
            get { return myFirstName; }
            set { myFirstName = value; }
        }

        /// <summary>
        /// Complete name of the person (equivalent to LastName FirstName)
        /// </summary>
        public string CompleteName
        {
            get { return myFirstName + " " + myLastName; }
        }

        /// <summary>
        /// Email of the person
        /// </summary>
        public string Email
        {
            get { return myEmail; }
            set { myEmail = value; }
        }

        /// <summary>
        /// Login of the person
        /// </summary>
        public string Login
        {
            get { return myLogin; }
            set { myLogin = value; }
        }

        /// <summary>
        /// Role of the person (1 = student 2 = professor)
        /// </summary>
        public int Role
        {
            get { return myRole; }
            set { myRole = value; }
        }
    }
}