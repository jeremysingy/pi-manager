using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.DataAccess
{
    public class Project
    {
        private int myId;
        private string myName;
        private string myDescription;
        private int myNbStudents;
        private int myClientID;
        private List<Person> myInscriptions;

        public Project(string name, string desc, int nbStudents)
        {
            myId = -1;
            myName = name;
            myDescription = desc;
            myNbStudents = nbStudents;
            myClientID = -1;
            myInscriptions = new List<Person>();
        }

        public Project(int id, string name, string desc, int nbStudents)
        {
            myId = id;
            myName = name;
            myDescription = desc;
            myNbStudents = nbStudents;
            myClientID = -1;
            myInscriptions = new List<Person>();

        }

        public int Id
        {
            get { return myId; }
            set { myId = value; }
        }

        public string Name
        {
            get { return myName; }
            set { myName = value; }
        }

        public string Description
        {
            get { return myDescription; }
            set { myDescription = value; }
        }

        public int NbStudents
        {
            get { return myNbStudents; }
            set { myNbStudents = value; }
        }

        public int ClientID
        {
            get { return myClientID; }
            set { myClientID = value; }
        }

        public List<Person> Inscriptions
        {
            get { return myInscriptions; }
            set { myInscriptions = value; }
        }

        public void AddPersonInInscriptions(Person person)
        {
            myInscriptions.Add(person);
        }

    }
}