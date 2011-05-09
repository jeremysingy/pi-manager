using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PIManager.DataAccess
{
    public class Technology
    {
        private int myId;
        private string myName;

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