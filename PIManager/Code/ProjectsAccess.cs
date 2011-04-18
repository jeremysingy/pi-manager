using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PIManager.Code
{
    public class ProjectsAccess
    {
        DBManager myDBManager = new DBManager();

        Project getProject(int id)
        {
            SqlDataReader reader = myDBManager.getProject(id);

            string name = reader.GetString(reader.GetOrdinal("name"));
            string desc = reader.GetString(reader.GetOrdinal("description"));
            int nbStudents = int.Parse(reader.GetString(reader.GetOrdinal("students")));

            return new Project(name, desc, 0);
        }
    }
}