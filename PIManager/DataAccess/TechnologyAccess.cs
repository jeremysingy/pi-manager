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

        public List<Technology> getTechnologies()
        {
            List<Technology> list = new List<Technology>();
            SqlDataReader reader = myDBManager.getTechnologies();

            while (reader.Read())
            {
                int id = (int)reader["pk_technology"];
                string name = (string)reader["name"];

                list.Add(new Technology(id, name));
            }

            reader.Close();

            return list;
        }
    }
}