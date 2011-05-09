﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

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
            SqlDataReader reader = myDBManager.getProjectList();

            // If there is no project available, return an empty list.
            if (!reader.HasRows) return new List<Project>();

            List<Project> projectList = new List<Project>();

            while (reader.Read())
            {
                int idProject = reader.GetInt32(reader.GetOrdinal("pk_project"));
                String titleProject = reader.GetString(reader.GetOrdinal("title"));
                int nbStudent = reader.GetInt32(reader.GetOrdinal("nbStudent"));
                
                Project project = new Project(idProject, titleProject, null, nbStudent);
                projectList.Add(project);
            }

            return projectList;
        }

        /// <summary>
        /// Gets list of inscriptions for a given person
        /// </summary>
        /// <param name="idPerson">id of person connected to the system</param>
        /// <returns>a list containing the project the person is subscribed to or an empty list </returns>
        public List<Int32> getInscriptions(int idPerson)
        {
            SqlDataReader reader = myDBManager.getInscriptions(idPerson);

            // We use a list to detect if a student has subscribe for a project or not.
            // By returning an empty list, this means there is no subscription for this person.
            List<Int32> inscriptionList = new List<Int32>();

            while (reader.Read())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("pk_project")))
                {
                    int idProject = reader.GetInt32(reader.GetOrdinal("pk_project"));
                    inscriptionList.Add(idProject);
                }
                
            }

            return inscriptionList;
        }

        /// <summary>
        /// Subscribe a student on a project.
        /// </summary>
        /// <param name="idPerson">id of the student</param>
        /// <param name="idProject">id of the project</param>
        /// <returns>true if inscription has been done, otherwise false</returns>
        public Boolean saveInscription(int idPerson, int idProject)
        {
            return myDBManager.inscriptionProjectTransaction(idPerson, idProject);
        }

        /// <summary>
        /// Unsubscribe a student from a project.
        /// </summary>
        /// <param name="idPerson">id of the student</param>
        /// <returns>true if subscription has been canceled, otherwise false</returns>
        public Boolean cancelInscription(int idPerson)
        {
            return myDBManager.cancelInscriptionTransaction(idPerson);
        }

        /// <summary>
        /// Adds a document to a project
        /// </summary>
        /// <param name="pk_project">id of the project</param>
        /// <param name="file">file to add</param>
        /// <returns>true if adding has been done, otherwise false</returns>
        public Boolean addDocument(int pk_project, HttpPostedFile file)
        {
            return myDBManager.addDocumentTransaction(pk_project, file);
        }

        public List<Project> getProjects()
        {
            List<Project> list = new List<Project>();
            SqlDataReader reader = myDBManager.getProjects();

            while (reader.Read())
            {
                int id = (int)reader["pk_project"];
                string name = (string)reader["title"];
                string desc = "desc basdfads";
                int nbStudents = (int)reader["nbstudents"];

                list.Add(new Project(id, name, desc, nbStudents));
            }

            reader.Close();

            return list;
        }

        public Project getProject(int id)
        {
            SqlDataReader reader = myDBManager.getProject(id);
            reader.Read();

            string name = (string)reader["title"];
            string desc = (string)reader["description"];
            int nbStudents = (int)reader["nbstudents"];

            return new Project(id, name, desc, nbStudents);
        }

        public Hashtable getProjectInscriptions()
        {

            Hashtable projects = new Hashtable();

            SqlDataReader reader = myDBManager.getProjectInscriptions();

            while (reader.Read())
            {
                int id = (int)reader["pk_project"];
                string name = (string)reader["title"];
                string firstname = (string)reader["firstname"];
                string lastname = (string)reader["lastname"];

                if (projects.ContainsKey(id))
                {
                    Project project = (Project)projects[id];
                    Person person = new Person(0, lastname, firstname, "", "", 1);
                    project.AddPersonInInscriptions(person);
                    projects.Remove(id);
                    projects.Add(id, project);
                }
                else
                {
                    Project project = new Project(id, name, "", 0);
                    Person person = new Person(0, lastname, firstname, "", "", 1);

                    project.AddPersonInInscriptions(person);

                    projects.Add(id, project);
                }

            }

            reader.Close();

            return projects;
        }
    }
}