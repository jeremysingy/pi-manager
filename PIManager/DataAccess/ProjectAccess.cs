using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PIManager.DataAccess
{
    /// <summary>
    /// This class gets information about projects from the database.
    /// </summary>
    public class ProjectAccess
    {
        private DBManager myDBManager;

        public ProjectAccess()
        {
            myDBManager = new DBManager();
        }

        /// <summary>
        /// Gets a list of projects that are opened for the inscription.
        /// </summary>
        /// <returns>List of projects opened for inscription</returns>
        public List<Project> getProjectList()
        {


            return null;
        }

        public List<Project> getProjects()
        {
            List<Project> list = new List<Project>();
            SqlDataReader reader = myDBManager.getProjects();

            while(reader.Read())
            {
                string name = reader.GetString(reader.GetOrdinal("title"));
                string desc = "desc basdfads";
                int nbStudents = int.Parse(reader.GetString(reader.GetOrdinal("nbstudents")));

                list.Add(new Project(name, desc, nbStudents));
            }

            reader.Close();

            return list;
        }

        Project getProject(int id)
        {
            SqlDataReader reader = myDBManager.getProjects();
            reader.Read();

            string name = reader.GetString(reader.GetOrdinal("name"));
            string desc = reader.GetString(reader.GetOrdinal("description"));
            //int nbStudents = int.Parse(reader.GetString(reader.GetOrdinal("students")));
            int nbStudents = reader.GetInt32(reader.GetOrdinal("nb_students"));

            return new Project(name, desc, nbStudents);
        }
    }
}