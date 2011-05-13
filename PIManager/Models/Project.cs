using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.Models
{
    /// <summary>
    /// Represent a project
    /// </summary>
    public class Project
    {
        private int myId;
        private string myName;
        private string myAbreviation;
        private string myDescription;
        private int myNbStudents;
        private List<int> myTechnologyIds = new List<int>();
        private int myClientId;
        private List<Person> myInscriptions = new List<Person>();
        private int myParentId;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="abreviation">Abreviation of the project</param>
        /// <param name="desc">Description of the project</param>
        /// <param name="nbStudents">Number of students for this project</param>
        public Project(string name, string abreviation, string desc, int nbStudents) :
            this(name, abreviation, desc, nbStudents, -1)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="abreviation">Abreviation of the project</param>
        /// <param name="desc">Description of the project</param>
        /// <param name="nbStudents">Number of students for this project</param>
        /// <param name="clientId">Id of the client of this project</param>
        public Project(string name, string abreviation, string desc, int nbStudents, int clientId) :
            this(-1, name, abreviation, desc, nbStudents, clientId)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="abreviation">Abreviation of the project</param>
        /// <param name="desc">Description of the project</param>
        /// <param name="nbStudents">Number of students for this project</param>
        /// <param name="clientId">Id of the client of this project</param>
        /// <param name="parentId">Id of the parent of this project</param>
        public Project(string name, string abreviation, string desc, int nbStudents, int clientId, int parentId) :
            this(-1, name, abreviation, desc, nbStudents, clientId, parentId)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="name">Name of the project</param>
        /// <param name="abreviation">Abreviation of the project</param>
        /// <param name="desc">Description of the project</param>
        /// <param name="nbStudents">Number of students for this project</param>
        /// <param name="clientId">Id of the client of this project</param>
        public Project(int id, string name, string abreviation, string desc, int nbStudents, int clientId) :
            this(id, name, abreviation, desc, nbStudents, clientId, -1)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the project</param>
        /// <param name="name">Name of the project</param>
        /// <param name="abreviation">Abreviation of the project</param>
        /// <param name="desc">Description of the project</param>
        /// <param name="nbStudents">Number of students for this project</param>
        /// <param name="clientId">Id of the client of this project</param>
        /// <param name="parentId">Id of the parent of this project</param>
        public Project(int id, string name, string abreviation, string desc, int nbStudents, int clientId, int parentId)
        {
            myId = id;
            myName = name;
            myAbreviation = abreviation;
            myDescription = desc;
            myNbStudents = nbStudents;
            myClientId = clientId;
            myParentId = parentId;
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

        public int ClientId
        {
            get { return myClientId; }
            set { myClientId = value; }
        }

        public int ParentId
        {
            get { return myParentId; }
            set { myParentId = value; }
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
            Console.WriteLine(myClientId + " and " + other.ClientId);

            return myName == other.Name &&
                   myAbreviation == other.Abreviation &&
                   myDescription == other.Description &&
                   myNbStudents == other.NbStudents &&
                   myClientId == other.ClientId;
        }
    }
}