using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Linq;

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
            SqlDataReader dataReader = myDBManager.getProjectList();

            // If there is no project available, return an empty list.
            if (!dataReader.HasRows) return new List<Project>();

            List<Project> projectList = new List<Project>();

            // get column indexes
            int idxIdProject = dataReader.GetOrdinal("pk_project");
            int idxDescriptionXML = dataReader.GetOrdinal("description_XML");
            int idxNbStudent = dataReader.GetOrdinal("nbStudent");

            while (dataReader.Read())
            {
                int idProject = dataReader.GetInt32(idxIdProject);

                SqlXml descriptionXML = dataReader.GetSqlXml(idxDescriptionXML);
                XDocument xml = XDocument.Load(descriptionXML.CreateReader());
                
                XElement titleElement = xml.Element("title");
                String titleProject = titleElement.Value;

                int nbStudent = dataReader.GetInt32(idxNbStudent);
                
                Project project = new Project(titleProject, null, nbStudent);
                projectList.Add(project);
            }

            return projectList;
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