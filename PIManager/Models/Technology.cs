using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.Models
{
    /// <summary>
    /// Represent a technology
    /// </summary>
    public class Technology
    {
        private int myId;
        private string myName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Id of the technology</param>
        /// <param name="name">Name of the technology</param>
        public Technology(int id, string name)
        {
            myId = id;
            myName = name;
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
    }
}