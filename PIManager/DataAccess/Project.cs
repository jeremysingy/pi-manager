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
        private string myAbreviation;
        private string myDescription;
        private int myNbStudents;
        private List<int> myTechnologyIds = new List<int>();
        private int myClientID;
        private List<Person> myInscriptions = new List<Person>();

        public Project(string name, string abreviation, string desc, int nbStudents) :
            this(name, abreviation, desc, nbStudents, -1)
        {

        }

        public Project(string name, string abreviation, string desc, int nbStudents, int clientId) :
            this(-1, name, abreviation, desc, nbStudents, clientId)
        {

        }

        public Project(int id, string name, string abreviation, string desc, int nbStudents, int clientId)
        {
            myId = id;
            myName = name;
            myAbreviation = abreviation;
            myDescription = desc;
            myNbStudents = nbStudents;
            myClientID = clientId;
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

        public string Abreviation
        {
            get { return myAbreviation; }
            set { myAbreviation = value; }
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

        public List<int> TechnologyIds
        {
            get { return myTechnologyIds; }
        }

        public void AddTechnologyId(int id)
        {
            myTechnologyIds.Add(id);
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

        public bool isEquivalent(Project other)
        {
            Console.WriteLine(myName + " and " + other.Name);
            Console.WriteLine(myAbreviation + " and " + other.Abreviation);
            Console.WriteLine(myDescription + " and " + other.Description);
            Console.WriteLine(myNbStudents + " and " + other.NbStudents);
            Console.WriteLine(myClientID + " and " + other.ClientID);

            return myName == other.Name &&
                   myAbreviation == other.Abreviation &&
                   myDescription == other.Description &&
                   myNbStudents == other.NbStudents &&
                   myClientID == other.ClientID;
        }
    }
}