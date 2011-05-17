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
using PIManager.Models;
using System.IO;

namespace PIManager.DAO
{
    /// <summary>
    /// Gets information about projects from the database in the model form.
    /// This is a Data Access Object (DAO)
    /// </summary>
    public class ProjectAccess
    {
        /// <summary>
        /// The manager used to connect to the database
        /// </summary>
        private DBManager myDBManager;

        /// <summary>
        /// Constructor
        /// </summary>
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
                           "description_XML.value(N'(//student)[1]', 'INT') AS nbStudent " +
                           "FROM pimanager.dbo.Project INNER JOIN pimanager.dbo.Period ON pimanager.dbo.Project.pk_period = pimanager.dbo.Period.pk_period " +
                           "WHERE description_XML.value(N'(//student)[1]', 'INT') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project) " +
                           "AND GETDATE() BETWEEN pimanager.dbo.Period.date_open AND pimanager.dbo.Period.date_close;";

            List<Project> projectList = new List<Project>();

            using (SqlConnection connection = myDBManager.newConnection())
            {
                SqlTransaction transaction = null;
                SqlDataReader reader = null;
                
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getProjectList");

                    reader = myDBManager.doSelect(query, connection, transaction, new Dictionary<string, object>());

                    // if there is no project available, return an empty list.
                    if (!reader.HasRows) return projectList;

                    while (reader.Read())
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
                catch (Exception exception)
                {
                    if (connection != null)
                    {
                        if (reader != null && !reader.IsClosed)
                            reader.Close();

                        if(transaction != null)
                            transaction.Rollback();
                    }
                    DBManager.getLog().Error("Error while getting project list: " + exception.Message);
                }
            }

            return projectList;
        }

        /// <summary>
        /// Gets list of inscriptions for a given person
        /// </summary>
        /// <param name="idPerson">Id of a person connected to the system</param>
        /// <returns>A list containing the project the person is subscribed to or an empty list </returns>
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
                SqlTransaction transaction = null;
                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    reader = myDBManager.doSelect(query, connection, transaction, param);

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
                catch (Exception exception)
                {
                    if (connection != null)
                    {
                        if (reader != null && !reader.IsClosed)
                            reader.Close();

                        if(transaction != null)
                            transaction.Rollback();
                    }

                    DBManager.getLog().Error("Error getting inscriptions of student: " + exception.Message);
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
            bool saveDone = false;
            string query = "SELECT count(*) FROM Project WHERE pk_project = @PK_PROJECT" +
                           " AND description_XML.value(N'(//student)[1]', 'integer') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project);";
            
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@PK_PROJECT", idProject);

            using (SqlConnection connection = myDBManager.newConnection())
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                    // Checks if the project is still available.
                    int available = (int)myDBManager.doSelectScalar(query, connection, transaction, param);
                    if (available != 0)
                    {
                        // Checks if the student is still not subscribed to a project
                        query = "SELECT pk_project FROM Person WHERE pk_person = @PK_PERSON;";
                        param.Clear();
                        param.Add("@PK_PERSON", idPerson);

                        object personProject = myDBManager.doSelectScalar(query, connection, transaction, param);
                        if (personProject == DBNull.Value)
                        {
                            // saves the inscription of the student on the project
                            query = "UPDATE Person SET pk_project = @PK_PROJECT WHERE pk_person = @PK_PERSON;";
                            param.Clear();
                            param.Add("@PK_PROJECT", idProject);
                            param.Add("@PK_PERSON", idPerson);

                            int affected = myDBManager.doUpdate(query, connection, transaction, param);
                            if (affected != 1) 
                                throw new Exception("The update of the person couldn't be done.");
                            else 
                                saveDone = true;
                        }
                    }

                    // In case project isn't available or student has already subscribed for a project...
                    if (saveDone)  
                        transaction.Commit();
                    else
                        throw new Exception("The project isn't available.");
                }
                catch (Exception exception)
                {
                    if (connection != null)
                        if (transaction != null)
                            transaction.Rollback();

                    DBManager.getLog().Error("Error saving inscription of student: " + exception.Message);
                }
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
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted);

                    int affected = myDBManager.doUpdate(query, connection, transaction, param);
                    if (affected == 1)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                        throw new Exception("The update of the person couldn't be done.");
                }
                catch (Exception exception)
                {
                    if (connection != null)
                        if (transaction != null)
                            transaction.Rollback();

                    DBManager.getLog().Error("Error saving inscription of student: " + exception.Message);
                    
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
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction(IsolationLevel.Serializable);
                    int affected = myDBManager.doUpdate(query, connection, transaction, param);
                    if (affected == 1)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                        throw new Exception("The document couldn't be added to the project.");
                }
                catch (Exception exception)
                {
                    if (connection != null)
                        if (transaction != null)
                            transaction.Rollback();

                    DBManager.getLog().Error("Error saving inscription of student: " + exception.Message);

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
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Period WHERE GETDATE() BETWEEN date_open AND date_close;";
                    transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "periodInscriptionOpen");

                    int opened = (int)myDBManager.doSelectScalar(query, connection, transaction, new Dictionary<string, object>());

                    transaction.Commit();

                    return opened != 0;
                }
                catch (Exception exception)
                {
                    if (connection != null)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                    }

                    DBManager.getLog().Error("Error saving inscription of student: " + exception.Message);

                    return false;
                }
            }
        }

        /// <summary>
        /// Get all the projects that are not opened to show, modify or inherit them
        /// </summary>
        /// <returns>A list of projects</returns>
        public List<Project> getProjects()
        {
            // Query for the attributes of the projects
            string query = "SELECT pk_project, " +
                "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                "description_xml.query('//description/*') AS description, " +
                "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                "pk_person " +
                "FROM pimanager.dbo.Project " +
                "WHERE pk_period is NULL";

            List<Project> list = new List<Project>();

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, new Dictionary<string,object>());

                while (reader.Read())
                {
                    int id = (int)reader["pk_project"];
                    string name = (string)reader["title"];
                    string abreviation = (string)reader["abreviation"];
                    string desc = (string)reader["description"];
                    int nbStudents = (int)reader["nbstudents"];
                    int clientId = (int)reader["pk_person"];

                    list.Add(new Project(id, name, abreviation, desc, nbStudents, clientId));
                }

                reader.Close();
                transaction.Commit();
            }

            return list;
        }

        /// <summary>
        /// Get a project with all its informations
        /// </summary>
        /// <param name="id">The id of hte project to retrieve</param>
        /// <returns>A project with its informations</returns>
        public Project getProject(int id)
        {
            // Query for the attributes of the project
            string query = "SELECT pk_project, " +
                "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                "description_xml.query('//description/*') AS description, " +
                "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                "image, pk_person, pk_parent " +
                "FROM Project " +
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

                if (!reader.Read())
                    throw new Exception("No project with id '" + id + "'");

                string name = (string)reader["title"];
                string abreviation = (string)reader["abreviation"];
                string desc = (string)reader["description"];
                int nbStudents = (int)reader["nbstudents"];
                int clientId = (int)reader["pk_person"];
                int parentId = reader.IsDBNull(reader.GetOrdinal("pk_parent")) ? -1 : (int)reader["pk_parent"];
                bool hasImage = !reader.IsDBNull(reader.GetOrdinal("image"));

                reader.Close();

                // Get the technologies of the project
                SqlDataReader readerTechnos = myDBManager.doSelect(queryTechnos, connection, transaction, param);

                Project project = new Project(id, name, abreviation, desc, nbStudents, clientId, parentId, hasImage);

                while (readerTechnos.Read())
                    project.AddTechnologyId((int)readerTechnos["pk_techno"]);

                readerTechnos.Close();
                transaction.Commit();

                return project;
            }
        }

        /// <summary>
        /// Add a new project to the database
        /// </summary>
        /// <param name="newProject">The new project to add</param>
        /// <param name="projectTechnos">The list if technologies associated with this new project</param>
        /// <param name="rawImage">Bytes representing the image posted to add to the description of the project</param>
        public void addProject(Project newProject, List<Technology> projectTechnos, byte[] rawImage)
        {
            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                List<SqlParameter> paramCreateProj = new List<SqlParameter>();
                paramCreateProj.Add(new SqlParameter("@title", newProject.Name));
                paramCreateProj.Add(new SqlParameter("@abreviation", newProject.Abreviation));
                paramCreateProj.Add(new SqlParameter("@nbstudents", newProject.NbStudents));
                paramCreateProj.Add(new SqlParameter("@description", newProject.Description));

                if (rawImage.Length == 0)
                {
                    SqlParameter nullParam = new SqlParameter("@image", SqlDbType.VarBinary);
                    nullParam.Value = DBNull.Value;
                    paramCreateProj.Add(nullParam);
                }
                else
                    paramCreateProj.Add(new SqlParameter("@image", rawImage));
                
                paramCreateProj.Add(new SqlParameter("@clientid", newProject.ClientId));

                // Output parameter to get the new id
                SqlParameter paramId = new SqlParameter("@createdid", 0);
                paramId.Direction = ParameterDirection.Output;
                paramCreateProj.Add(paramId);

                if(newProject.ParentId == -1)
                    paramCreateProj.Add(new SqlParameter("@parentid", DBNull.Value));
                else
                    paramCreateProj.Add(new SqlParameter("@parentid", newProject.ParentId));

                myDBManager.executeProcedure("create_project", connection, transaction, paramCreateProj);

                List<SqlParameter> paramUpdateTechnos = new List<SqlParameter>();
                paramUpdateTechnos.Add(new SqlParameter("@id", paramId.Value));
                paramUpdateTechnos.Add(new SqlParameter("@technologies", transformTechnologies(projectTechnos)));

                myDBManager.executeProcedure("update_technos", connection, transaction, paramUpdateTechnos);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Modify an existing project from the database
        /// </summary>
        /// <param name="oldProject">The informations of the project before modifying it</param>
        /// <param name="newProject">The new informations to put for this project</param>
        /// <param name="projectTechnos">The list if technologies associated with this project</param>
        /// <param name="rawImage">Image posted to add to the description of the project</param>
        /// <returns>true if the project has been added, false otherwise (the project doesn't exist or has been modified before by someone else)</returns>
        public bool modifyProject(Project oldProject, Project newProject, List<Technology> projectTechnos, byte[] rawImage)
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
                param.Add("@id", oldProject.Id);

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

                Project crtProject = new Project(oldProject.Id, name, abreviation, desc, nbStudents, clientId);

                if (!oldProject.isEquivalent(crtProject))
                    return false;

                List<SqlParameter> paramUpdateProj = new List<SqlParameter>();
                paramUpdateProj.Add(new SqlParameter("@id", oldProject.Id));
                paramUpdateProj.Add(new SqlParameter("@title", newProject.Name));
                paramUpdateProj.Add(new SqlParameter("@abreviation", newProject.Abreviation));
                paramUpdateProj.Add(new SqlParameter("@nbstudents", newProject.NbStudents));
                paramUpdateProj.Add(new SqlParameter("@description", newProject.Description));

                if (rawImage.Length == 0)
                {
                    SqlParameter nullParam = new SqlParameter("@image", SqlDbType.VarBinary);
                    nullParam.Value = DBNull.Value;
                    paramUpdateProj.Add(nullParam);
                }
                else
                    paramUpdateProj.Add(new SqlParameter("@image", rawImage));

                paramUpdateProj.Add(new SqlParameter("@clientid", newProject.ClientId));

                myDBManager.executeProcedure("dbo.update_project", connection, transaction, paramUpdateProj);

                List<SqlParameter> paramUpdateTechnos = new List<SqlParameter>();
                paramUpdateTechnos.Add(new SqlParameter("@id", oldProject.Id));
                paramUpdateTechnos.Add(new SqlParameter("@technologies", transformTechnologies(projectTechnos)));

                myDBManager.executeProcedure("dbo.update_technos", connection, transaction, paramUpdateTechnos);
                transaction.Commit();

                return true;
            }
        }

        /// <summary>
        /// Delete an existing project from the database
        /// </summary>
        /// <param name="id">The id of the project to delete</param>
        public void deleteProject(int id)
        {
            string deleteQuery = "DELETE FROM Project WHERE pk_project = @id";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                myDBManager.doDelete(deleteQuery, connection, transaction, param);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Gets the image added to the given project.
        /// </summary>
        /// <param name="id">id of the project</param>
        /// <returns>image as byte array</returns>
        public byte[] getImage(int id)
        {
            // Query for the attributes of the projects
            string query = "SELECT image FROM Project WHERE pk_project = @id";

            byte[] rawImage = null;

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@id", id);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);
                reader.Read();

                object o = reader["image"];
                if(o != System.DBNull.Value)
                    rawImage = (byte[])o;

                reader.Close();
                transaction.Commit();
            }

            return rawImage;
        }

        /// <summary>
        /// Open a project to registration, so that students can register to
        /// </summary>
        /// <param name="ids">The ids of the projects to open</param>
        /// <param name="dateOpen">The open date period</param>
        /// <param name="dateClose">The closing date period</param>
        public void openRegistration(HashSet<int> ids, DateTime dateOpen, DateTime dateClose)
        {
            string insertQuery = "INSERT INTO Period (date_open, date_close, year) " +
                                 "VALUES(@date_open, @date_close, @year)";

            string updateQuery = "UPDATE Project SET pk_period = @idperiod WHERE pk_project IN (" + transformIds(ids) + ")";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

                Dictionary<string, object> paramInsert = new Dictionary<string, object>();
                paramInsert.Add("@date_open", dateOpen.ToString("yyyy-MM-dd HH:mm"));
                paramInsert.Add("@date_close", dateClose.ToString("yyyy-MM-dd HH:mm"));
                paramInsert.Add("@year", dateOpen.ToString("yyyy"));

                int id = myDBManager.doInsert(insertQuery, connection, transaction, paramInsert);

                Dictionary<string, object> paramUpdate = new Dictionary<string, object>();
                paramInsert.Add("@idperiod", id);

                myDBManager.doUpdate(updateQuery, connection, transaction, paramInsert);

                transaction.Commit();
            }
        }

        /// <summary>
        /// Get all registrations of all projects
        /// </summary>
        /// <returns>Hash table containing all the projects with registrations</returns>
        public Hashtable getProjectInscriptions()
        {
            Hashtable projects = new Hashtable();

            using (SqlConnection connection = myDBManager.newConnection())
            {

                string query = "SELECT pr.pk_project, description_xml.value('(//title)[1]', 'varchar(80)') AS title, firstname, lastname FROM project pr, person pe WHERE pr.pk_project = pe.pk_project";

                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showInscriptions");

                Dictionary<string, object> empty = new Dictionary<string,object>();

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, empty);

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
                transaction.Commit();
            }

            return projects;
        }
		
        /// <summary>
        /// Get description of all projects
        /// </summary>
        /// <returns>List of all projects</returns>
		public List<Project> getFullProject()
        {
            List<Project> projects = new List<Project>();

            string query = "SELECT pr.pk_project, description_xml.value('(//title)[1]', 'varchar(80)') AS title, pe.pk_person FROM project pr, person pe, period time WHERE pr.pk_person = pe.pk_person AND pr.pk_period = time.pk_period AND date_close < getdate()";

            using (SqlConnection connection = myDBManager.newConnection())
            {


                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showProjects");

                Dictionary<string, object> empty = new Dictionary<string, object>();

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, empty);

                while (reader.Read())
                {
                    int id = (int)reader["pk_project"];
                    string name = (string)reader["title"];
                    int clientID = (int)reader["pk_person"];
                    Project project = new Project(id, name, "", "", 0, clientID);
                    projects.Add(project);
                }

                reader.Close();
                transaction.Commit();
            }

            return projects;
        }

        /// <summary>
        /// Get the complete name of a person
        /// </summary>
        /// <param name="pk_person">Primary key of the person</param>
        /// <returns>Complete name of the person</returns>
        public string getPersonCompletName(int pk_person)
        {
            string personName = "";

            string query = "SELECT firstname, lastname FROM person  WHERE pk_person = @pk_person";

            using (SqlConnection connection = myDBManager.newConnection())
            {

                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showPeronName");

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("@pk_person", pk_person);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);

                while (reader.Read())
                {

                    string firstname = (string)reader["firstname"];
                    string lastname = (string)reader["lastname"];

                    personName += firstname + " " + lastname;

                }

                reader.Close();
                transaction.Commit();
            }

            return personName;
        }

        /// <summary>
        /// Get the group registered to a project
        /// </summary>
        /// <param name="pk_project">Id of the project</param>
        /// <returns>List of people registered to this project</returns>
        public List<Person> getProjectGroup(int pk_project)
        {
            List<Person> persons = new List<Person>();

            using (SqlConnection connection = myDBManager.newConnection())
            {

                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getProjectGroup");

                string query = "SELECT firstname, lastname FROM person  WHERE pk_project = @pk_project";

                Dictionary<string, object> param = new Dictionary<string, object>();

                param.Add("@pk_project", pk_project);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);

                while (reader.Read())
                {

                    string firstname = (string)reader["firstname"];
                    string lastname = (string)reader["lastname"];

                    persons.Add(new Person(0, lastname, firstname, "", "", 1));

                }

                reader.Close();

                transaction.Commit();
            }
            return persons;
        }

        /// <summary>
        /// Get all the technologies of a project
        /// </summary>
        /// <param name="pk_project">Primary key of the project</param>
        /// <returns>Technologies of the project</returns>
        public List<Technology> getTechnologyProject(int pk_project)
        {
            List<Technology> technologys = new List<Technology>();

            SqlConnection connection = myDBManager.newConnection();

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getTechnologyProject");

            string query = "SELECT name FROM project_techno prt, technology t  WHERE prt.pk_project = @pk_project AND prt.pk_techno = t.pk_technology";


            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("@pk_project", pk_project);

            SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);


            while (reader.Read())
            {

                string technology_name = (string)reader["name"];


                technologys.Add(new Technology(0, technology_name));

            }

            reader.Close();

            return technologys;
        }

        /// <summary>
        /// Get the history of one project
        /// </summary>
        /// <param name="choosed_project">Primary key of the project</param>
        /// <returns>Stack of the history</returns>
        public Stack<Project> getHistoryProject(int choosed_project)
        {
            Stack<Project> projectHistory = new Stack<Project>();

            string child_query = "SELECT pk_child, description_xml.value('(//title)[1]', 'varchar(80)') AS title  FROM project pr, project_speed ps WHERE ps.pk_project = @pk_project AND ps.pk_child = pr.pk_project";

            string parent_query = "SELECT pk_parent FROM project WHERE pk_project = @pk_project";

            string projectName_query = "SELECT description_xml.value('(//title)[1]', 'varchar(80)') AS title FROM project WHERE pk_project = @pk_project";

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showHistory");


                Dictionary<string, object> param_child_query = new Dictionary<string, object>();
                param_child_query.Add("@pk_project", choosed_project);

                SqlDataReader readerChild = myDBManager.doSelect(child_query, connection, transaction, param_child_query);
                //Child
                while (readerChild.Read())
                {
                    int id_project = (int)readerChild["pk_child"];
                    string project_name = (string)readerChild["title"];

                    projectHistory.Push(new Project(id_project, project_name, "", "", -1, -1));

                }

                readerChild.Close();


                //choosed
                Dictionary<string, object> param_projectName = new Dictionary<string, object>();
                param_projectName.Add("@pk_project", choosed_project);

                string projectName = (string)myDBManager.doSelectScalar(projectName_query, connection, transaction, param_projectName);

                projectHistory.Push(new Project(choosed_project, projectName, "", "", -1, -1));


                //parent
                int pk_current_project = choosed_project;

                bool finish = false;

                do
                {
                    Dictionary<string, object> param_parent_project = new Dictionary<string, object>();
                    param_parent_project.Add("@pk_project", pk_current_project);

                    object o = myDBManager.doSelectScalar(parent_query, connection, transaction, param_parent_project);

                    int id_project;

                    
                    if (!DBNull.Value.Equals(o))
                    {
                        id_project = (int)o;
                    }
                    else
                    {
                        finish = true;
                        break;
                    }

                    param_projectName = new Dictionary<string, object>();
                    param_projectName.Add("@pk_project", id_project);

                    string project_name = (string)myDBManager.doSelectScalar(projectName_query, connection, transaction, param_projectName);

                    pk_current_project = id_project;

                    projectHistory.Push(new Project(id_project, project_name, "", "", -1, -1));


                } while (!finish);


                transaction.Commit();
            }

            return projectHistory;
        }
        
         /// <summary>
        /// Transform a list of technologies in the XML form undestood by the stored procedure
        /// </summary>
        /// <param name="technologies">List of technologies to transform</param>
        /// <returns>A string in the proper XML form</returns>
        private string transformTechnologies(List<Technology> technologies)
        {
            string result = "";

            foreach (Technology techno in technologies)
                result += "<e>" + techno.Id.ToString() + "</e>";

            return result;
        }

        /// <summary>
        /// Transform a list of ids in SQL forme to put in a IN predicate
        /// </summary>
        /// <param name="ids">List of ids to transform</param>
        /// <returns>A string in the proper SQL form</returns>
        private string transformIds(HashSet<int> ids)
        {
            return String.Join(",", ids);
        }

    }
}