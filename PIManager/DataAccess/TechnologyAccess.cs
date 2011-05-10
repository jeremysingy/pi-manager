using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PIManager.DataAccess
{
    public class TechnologyAccess
    {
        private DBManager myDBManager;

        public TechnologyAccess()
        {
            myDBManager = new DBManager();
        }

        public Dictionary<int, Technology> getTechnologies()
        {
            Dictionary<int, Technology> list = new Dictionary<int, Technology>();
            SqlDataReader reader = myDBManager.getTechnologies();

            while (reader.Read())
            {
                int id = (int)reader["pk_technology"];
                string name = (string)reader["name"];

                list.Add(id, new Technology(id, name));
            }

            reader.Close();

            return list;
        }
    }
}