using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Collections.Specialized;
using System.Collections;
using System.IO;

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
            string query = "SELECT pk_project, " +
                           "description_XML.query('data(//title)') AS title, " +
                           "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                           "description_XML.value(N'(//student)[1]', 'integer') AS nbStudent " +
                           "FROM pimanager.dbo.Project " +
                           "WHERE description_XML.value(N'(//student)[1]', 'integer') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project);";

            List<Project> projectList = new List<Project>();

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                SqlDataReader reader = myDBManager.doSelect(query,connection,transaction,new Dictionary<string,object>());

                // if there is no project available, return an empty list.
                if(!reader.HasRows) return projectList;

                while(reader.Read()) 
                {
                    int idProject = (int)reader["pk_project"];
                    string titleProject = (string)reader["title"];
                    string abreviationProject = (string)reader["abreviation"];
                    int nbStudent = (int)reader["nbStudent"];

                    Project project = new Project(idProject, titleProject, abreviationProject, null, nbStudent, -1);
                    projectList.Add(project);
                }

                reader.Close();
                transaction.Commit();
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
            string query = "SELECT pk_project FROM Person WHERE pk_person = @PERSON;";

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@PERSON", idPerson);

            // We use a list to detect if a student has subscribe for a project or not.
            // By returning an empty list, this means there is no subscription for this person.
            List<int> inscriptionList = new List<int>();

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);

                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("pk_project")))
                    {
                        int idProject = (int)reader["pk_project"];
                        inscriptionList.Add(idProject);
                    }
                }
                reader.Close();
                transaction.Commit();
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
            bool saveDone = false;
            string query = "SELECT count(*) FROM Project WHERE pk_project = @PK_PROJECT" +
                           " AND description_XML.value(N'(//student)[1]', 'integer') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project);";
                        
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@PK_PROJECT", idProject);

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                // Checks if the project is still available.
                int available = (int)myDBManager.doSelectScalar(query, connection, transaction, param);
                if (available != 0)
                {
                    // Checks if the student is still not subscribed to a project
                    query = "SELECT pk_project FROM Person WHERE pk_person = @PK_PERSON;";
                    param.Clear();
                    param.Add("@PK_PERSON", idPerson);
    
                    object personProject = myDBManager.doSelectScalar(query, connection, transaction, param);
                    if (personProject != null)
                    {
                        // saves the inscription of the student on the project
                        query = "UPDATE Person SET pk_project = @PK_PROJECT WHERE pk_person = @PK_PERSON;";
                        param.Clear();
                        param.Add("@PK_PROJECT", idProject);
                        param.Add("@PK_PERSON", idPerson);

                        int affected = myDBManager.doUpdate(query, connection, transaction, param);
                        if (affected == 1)
                        {
                            transaction.Commit();
                            saveDone = true;
                        }
                    }
                }

                // In case project isn't available or student has already subscribed for a project...
                if (!saveDone)
                    transaction.Rollback();
            }

            return saveDone;
        }

        /// <summary>
        /// Unsubscribe a student from a project.
        /// </summary>
        /// <param name="idPerson">id of the student</param>
        /// <returns>true if subscription has been canceled, otherwise false</returns>
        public Boolean cancelInscription(int idPerson)
        {
            string query = "UPDATE Person SET pk_project = NULL WHERE pk_person = @PK_PERSON;";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@PK_PERSON", idPerson);

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                int affected = myDBManager.doUpdate(query, connection, transaction, param);
                if (affected == 1)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// Adds a document to a project
        /// </summary>
        /// <param name="pk_project">id of the project</param>
        /// <param name="file">file to add</param>
        /// <returns>true if adding has been done, otherwise false</returns>
        public Boolean addDocument(int pk_project, HttpPostedFile file)
        {
            // conversion of HttpPostedFile into byte[]
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }

            string query = "UPDATE Project SET documents = @DOCUMENT WHERE pk_project = @PROJECTID";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@DOCUMENT", fileData);
            param.Add("@PROJECTID", pk_project);

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                int affected = myDBManager.doUpdate(query, connection, transaction, param);
                if (affected == 1)
                {
                    transaction.Commit();
                    return true;
                }
                else
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        /// <summary>
        /// Checks if the period of inscription to the projects is still opened.
        /// </summary>
        /// <returns>true if it is opened, otherwise false</returns>
        public Boolean checkPeriodInscriptionOpen()
        {
            string currentDate = System.DateTime.Now.ToString();
            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Period WHERE @currentDate > date_open AND @currentDate < date_close;";
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "periodInscriptionOpen");
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;
                command.Parameters.Add("@currentDate", SqlDbType.DateTime).Value = currentDate;

                int opened = (int)command.ExecuteScalar();

                transaction.Commit();

                return opened != 0;
            }
        }

        public List<Project> getProjects()
        {
            List<Project> list = new List<Project>();
            SqlDataReader reader = myDBManager.getProjects();

            while (reader.Read())
            {
                int id = (int)reader["pk_project"];
                string name = (string)reader["title"];
                string abreviation = (string)reader["abreviation"];
                string desc = "desc basdfads";
                int nbStudents = (int)reader["nbstudents"];
                int clientId = (int)reader["pk_person"];

                list.Add(new Project(id, name, abreviation, desc, nbStudents, clientId));
            }

            reader.Close();

            return list;
        }

        public Project getProject(int id)
        {
            // Query for the attributes of the project
            string query = "SELECT pk_project, " +
                "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                "description_xml.query('//description/*') AS description, " +
                "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                "pk_person " +
                "FROM pimanager.dbo.Project " +
                "WHERE pk_project = @id";

            // Query for the technologies of the project
            string queryTechnos = "SELECT * FROM Project_Techno WHERE pk_project = @id";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                
                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                // Get the attributes of the project
                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);
                reader.Read();

                string name = (string)reader["title"];
                string abreviation = (string)reader["abreviation"];
                string desc = (string)reader["description"];
                int nbStudents = (int)reader["nbstudents"];
                int clientId = (int)reader["pk_person"];

                reader.Close();

                // Get the technologies of the project
                SqlDataReader readerTechnos = myDBManager.doSelect(queryTechnos, connection, transaction, param);

                Project project = new Project(id, name, abreviation, desc, nbStudents, clientId);

                while (readerTechnos.Read())
                    project.AddTechnologyId((int)readerTechnos["pk_techno"]);

                readerTechnos.Close();
                transaction.Commit();

                return project;
            }
        }

        public bool modifyProject(int id, Project oldProject, Project newProject, List<Technology> projectTechnos)
        {
            // Query to get the current project stored in DB
            string selectQuery = "SELECT pk_project, " +
                "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                "description_xml.query('//description/*') AS description, " +
                "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                "pk_person " +
                "FROM pimanager.dbo.Project " +
                "WHERE pk_project = @id";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                // Get the current project in DB
                SqlDataReader reader = myDBManager.doSelect(selectQuery, connection, transaction, param);

                if (!reader.Read())
                    return false;

                string name = (string)reader["title"];
                string abreviation = (string)reader["abreviation"];
                string desc = (string)reader["description"];
                int nbStudents = (int)reader["nbstudents"];
                int clientId = (int)reader["pk_person"];
                reader.Close();

                Project crtProject = new Project(id, name, abreviation, desc, nbStudents, clientId);

                if (!oldProject.isEquivalent(crtProject))
                    return false;

                Dictionary<string, object> paramUpdateProj = new Dictionary<string, object>();
                paramUpdateProj.Add("@id", id);
                paramUpdateProj.Add("@title", newProject.Name);
                paramUpdateProj.Add("@abreviation", newProject.Abreviation);
                paramUpdateProj.Add("@nbstudents", newProject.NbStudents);
                paramUpdateProj.Add("@description", newProject.Description);
                paramUpdateProj.Add("@clientid", newProject.ClientID);

                myDBManager.executeProcedure("dbo.update_project", connection, transaction, paramUpdateProj);

                Dictionary<string, object> paramUpdateTechnos = new Dictionary<string, object>();
                paramUpdateTechnos.Add("@id", id);
                paramUpdateTechnos.Add("@technologies", transformTechnologies(projectTechnos));

                myDBManager.executeProcedure("pimanager.dbo.update_technos", connection, transaction, paramUpdateTechnos);

                transaction.Commit();

                return true;
            }
        }

        public void deleteProject(int id)
        {
            string deleteQuery = "DELETE FROM Project WHERE pk_project = @id";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                myDBManager.doDelete(deleteQuery, connection, transaction, param);

                transaction.Commit();
            }
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
                    Project project = new Project(id, name, "", "", 0, -1);
                    Person person = new Person(0, lastname, firstname, "", "", 1);

                    project.AddPersonInInscriptions(person);

                    projects.Add(id, project);
                }

            }

            reader.Close();

            return projects;
        }

        private string transformTechnologies(List<Technology> myProjectTechnos)
        {
            string result = "";

            foreach (Technology techno in myProjectTechnos)
                result += "<e>" + techno.Id.ToString() + "</e>";

            return result;
        }
    }
}