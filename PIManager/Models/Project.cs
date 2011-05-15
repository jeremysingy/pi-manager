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
		private List<Technology> myTechnology = new List<Technology>();
        private bool myHasImage;

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
        public Project(int id, string name, string abreviation, string desc, int nbStudents, int clientId, int parentId) :
            this(id, name, abreviation, desc, nbStudents, clientId, parentId, false)
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
        /// <param name="hasImage">true if the project has an image, false otherwise</param>
        public Project(int id, string name, string abreviation, string desc, int nbStudents, int clientId, int parentId, bool hasImage)
        {
            myId = id;
            myName = name;
            myAbreviation = abreviation;
            myDescription = desc;
            myNbStudents = nbStudents;
            myClientId = clientId;
            myParentId = parentId;
            myHasImage = hasImage;
        }

        /// <summary>
        /// Id of the project
        /// </summary>
        public int Id
        {
            get { return myId; }
            set { myId = value; }
        }

        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name
        {
            get { return myName; }
            set { myName = value; }
        }

        /// <summary>
        /// Abreviation of the project
        /// </summary>
        public string Abreviation
        {
            get { return myAbreviation; }
            set { myAbreviation = value; }
        }

        /// <summary>
        /// Description of the project
        /// </summary>
        public string Description
        {
            get { return myDescription; }
            set { myDescription = value; }
        }

        /// <summary>
        /// Maximum number of students for the project
        /// </summary>
        public int NbStudents
        {
            get { return myNbStudents; }
            set { myNbStudents = value; }
        }

        /// <summary>
        /// Ids of the technologies of this project
        /// </summary>
        public List<int> TechnologyIds
        {
            get { return myTechnologyIds; }
        }

        /// <summary>
        /// Add a technology id to this project
        /// </summary>
        /// <param name="id"></param>
        public void AddTechnologyId(int id)
        {
            myTechnologyIds.Add(id);
        }

        /// <summary>
        /// Client id of this project
        /// </summary>
        public int ClientId
        {
            get { return myClientId; }
            set { myClientId = value; }
        }

        /// <summary>
        /// Parent id of this project
        /// </summary>
        public int ParentId
        {
            get { return myParentId; }
            set { myParentId = value; }
        }

        /// <summary>
        /// List of registered people to this project
        /// </summary>
        public List<Person> Inscriptions
        {
            get { return myInscriptions; }
            set { myInscriptions = value; }
        }

        /// <summary>
        /// true if the project has an image, false otherwise
        /// </summary>
        public bool HasImage
        {
            get { return myHasImage; }
            set { myHasImage = value; }
           }
           
           public void AddPersonInInscriptions(Person person)
        {
            myInscriptions.Add(person);
        }


        public List<Technology> Technology
        {
            get { return myTechnology; }
            set { myTechnology = value; }
        }
		
		public void AddTechnologyInMyTechnology(Technology technology)

        /// <summary>
        /// Add a registered person to this project
        /// </summary>
        /// <param name="person">The person to add</param>
        public void AddPersonInInscriptions(Person person)
        {
            myInscriptions.Add(person);
        }

        /// <summary>
        /// Return true if this project is equivalent to the other project.
        /// Equivalent means to the two projects have the same properties but
        /// not neccessarily the same id.
        /// </summary>
        /// <param name="other">The project to test with this one</param>
        /// <returns>True if this project is equivalent, false otherwise</returns>
        public bool isEquivalent(Project other)
        {
            return myName == other.Name &&
                   myAbreviation == other.Abreviation &&
                   myDescription == other.Description &&
                   myNbStudents == other.NbStudents &&
                   myClientId == other.ClientId;
        }
    }
}