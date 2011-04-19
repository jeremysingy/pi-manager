using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.DataAccess
{
    public class Project
    {
        private string myName;
        private string myDescription;
        private int myNbStudents;
        private string desc;
        private int p;

        public Project(string name, string desc, int nbStudents)
        {
            myName = name;
            myDescription = desc;
            myNbStudents = nbStudents;
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
    }
}