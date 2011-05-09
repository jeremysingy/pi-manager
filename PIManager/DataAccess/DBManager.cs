using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace PIManager.DataAccess
{
    /// <summary>
    /// This class manages accesses to the database.
    /// </summary>
    public class DBManager
    {
        public readonly string DB_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PIDBConnection"].ToString();
        

        /*
         * Example of prepared statement
         * 
         * SqlCommand command = new SqlCommand(null, connection);
         * command.CommandText = "SELECT pk_project, description_xml from Project having count(select * from Person where fk_project = pk_project) < nbStudent;";
         * command.Parameters.Add("@id", 20); // for example...
         * command.Prepare();
         */
        

        /// <summary>
        /// Gets the list of projects opened for the inscription.
        /// </summary>
        /// <returns>list of projects with id, title and number of students</returns>
        public SqlDataReader getProjectList()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            String query = "SELECT pk_project, " +
                           "description_XML.query('data(//title)') AS title, " +
                           "description_XML.value(N'(//student)[1]', 'integer') AS nbStudent " +
                           "FROM pimanager.dbo.Project " +
                           "WHERE description_XML.value(N'(//student)[1]', 'integer') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project);";

            connection.Open();
            SqlCommand command = connection.CreateCommand();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "projectListStudent");
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = query;
            
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Gets list of inscriptions for a given person.
        /// </summary>
        /// <param name="idPerson">id of the person connected to the system</param>
        /// <returns>id of the project or null if no inscription</returns>
        public SqlDataReader getInscriptions(int idPerson)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            String query = "SELECT pk_project FROM Person WHERE pk_person = @PERSON;";

            connection.Open();

            SqlCommand command = connection.CreateCommand();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "nbInscriptionStudent");
            command.Connection = connection;
            command.Transaction = transaction;
            command.CommandText = query;
            command.Parameters.Add("@PERSON", SqlDbType.Int).Value = idPerson;

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// Executes the SQL transaction inserting a subscription of a person to a project.
        /// This transaction is in isolation level "serializable" to avoid having too much 
        /// students subscribed to a project. 
        /// </summary>
        /// <param name="pk_person">id of the student</param>
        /// <param name="pk_project">id of the project</param>
        /// <returns>true if the subscription was done, otherwise false</returns>
        public Boolean inscriptionProjectTransaction(int pk_person, int pk_project)
        {
            Boolean saveDone = false; // maintain result of the save
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = new SqlConnection(DB_CONNECTION_STRING);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
            
                // starts local transaction
                transaction = connection.BeginTransaction(IsolationLevel.Serializable, "inscriptionProject");
                command.Connection = connection;
                command.Transaction = transaction;
            
                // checks if the project is still available
                command.CommandText = "SELECT count(*) FROM Project WHERE pk_project = " + pk_project + 
                                      " AND description_XML.value(N'(//student)[1]', 'integer') > (SELECT COUNT(*) FROM Person WHERE Person.pk_project = Project.pk_project);";
                
                Int32 available = (Int32)command.ExecuteScalar();
                if (available != 0)
                {
                    // checks if the student is still not subscribed to a project
                    command.CommandText = "SELECT pk_project FROM Person WHERE pk_person = " + pk_person + ";";

                    if (command.ExecuteScalar() != null)
                    {
                        // savves the inscription of the student on the project
                        command.CommandText = "UPDATE Person SET pk_project = " + pk_project + " WHERE pk_person = " + pk_person + ";";
                        int affected = command.ExecuteNonQuery();
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

                connection.Close();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                    if (connection != null) 
                        connection.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return saveDone;
        }

        /// <summary>
        /// Cancel a subscription for a student.
        /// </summary>
        /// <param name="pk_person"></param>
        /// <returns></returns>
        public Boolean cancelInscriptionTransaction(int pk_person)
        {
            Boolean cancelDone = false; // maintain result of the cancel
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = new SqlConnection(DB_CONNECTION_STRING);
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                string query = "UPDATE Person SET pk_project = NULL WHERE pk_person = " + pk_person;

                // starts local transaction
                transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted, "unsubscribe");
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;

                int affected = command.ExecuteNonQuery();
                if (affected == 1)
                {
                    transaction.Commit();
                    cancelDone = true;
                }
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                    if (connection != null)
                        connection.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return cancelDone;
        }

        /// <summary>
        /// Adds a document to a given project.
        /// </summary>
        /// <param name="pk_project">id of the project</param>
        /// <param name="file">file to add</param>
        /// <returns>true if add was done; otherwise false</returns>
        public Boolean addDocumentTransaction(int pk_project, HttpPostedFile file)
        {
            Boolean addDone = false; // maintain result of the add
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            // conversion of HttpPostedFile into byte[]
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream)) {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }

            try
            {
                connection = new SqlConnection(DB_CONNECTION_STRING);
                connection.Open();
                SqlCommand command = connection.CreateCommand();

                string query = "UPDATE Project SET documents = @DOCUMENT WHERE pk_project = @PROJECTID";

                // starts local transaction
                transaction = connection.BeginTransaction(IsolationLevel.Serializable, "addProjectDocument");
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandText = query;
                command.Parameters.Add("@DOCUMENT", SqlDbType.VarBinary).Value = fileData;
                command.Parameters.Add("@PROJECTID", SqlDbType.Int).Value = pk_project;

                int affected = command.ExecuteNonQuery();
                if (affected == 1)
                {
                    transaction.Commit();
                    addDone = true;
                }
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                    if (connection != null)
                        connection.Close();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return addDone;
        }

        public SqlDataReader getProjects()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                            "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                            "description_xml.query('//description') AS description, " +
                            "description_xml.value('(//student)[1]', 'int') AS nbstudents " +
                            "FROM pimanager.dbo.Project";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader getProject(int id)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                            "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                            "description_xml.query('//description') AS description, " +
                            "description_xml.value('(//student)[1]', 'int') AS nbstudents " +
                            "FROM pimanager.dbo.Project " +
                            "WHERE pk_project = @id";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public void addProject(string name, string desc, int nStudents)
        {

        }

        public void openRegistration()
        {

        }


        private void insertQuery(string query, SqlTransaction transaction)
        {
            query += " SET @newId = SCOPE_IDENTITY()";

            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            //SqlTransaction transaction = connection.BeginTransaction(isolationLevel);

            SqlCommand command = new SqlCommand(query, connection, transaction);

            SqlParameter idParam = new SqlParameter("@newId", SqlDbType.Int);
            idParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(idParam);

            command.Connection.Open();
            command.ExecuteNonQuery();

            //transaction.Commit();
        }

        private SqlDataReader selectQuery(string query, SqlTransaction transaction)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            SqlCommand command = new SqlCommand(query, connection, transaction);

            command.Connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public int getPerson(string login, string pass)
        {
            int pk_person = -1;

            string query = "SELECT pk_person FROM Person WHERE login like @login and password like @pass;";

            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING); //TODO change with anonymous user with limited rigth

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "Login");

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@pass", pass);

                SqlDataReader sqldatareader = command.ExecuteReader();

                while (sqldatareader.Read())
                {
                    pk_person = sqldatareader.GetInt32(0);
                }


            }
            catch (SqlException sqlError)
            {
                transaction.Rollback();
            }

            connection.Close();


            return pk_person;

        }


        public int getPersonType(int pk_person)
        {
            int person_type = -1;

            string query = "SELECT role FROM Person WHERE pk_person = @pk_person;";

            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING); //TODO change with anonymous user with limited rigth

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getTypePerson");

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@pk_person", pk_person);

                SqlDataReader sqldatareader = command.ExecuteReader();

                while (sqldatareader.Read())
                {
                    person_type = sqldatareader.GetInt32(0);
                }

                sqldatareader.Close();
            }
            catch (SqlException sqlError)
            {
                transaction.Rollback();
            }

            

            transaction.Commit();

            connection.Close();


            return person_type;

        }

        public SqlDataReader getTechnologies()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT * FROM Technology";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader getPersons(int role)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT * FROM Person WHERE role = @role";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@role", role);

            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader getProjectInscriptions()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showInscriptions");

            string query = "SELECT pr.pk_project, description_xml.value('(//title)[1]', 'varchar(80)') AS title, firstname, lastname FROM project pr, person pe WHERE pr.pk_project = pe.pk_project";

            SqlDataReader sqldatareader = null;

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                sqldatareader = command.ExecuteReader(CommandBehavior.CloseConnection);

                
            }
            catch (SqlException sqlError)
            {
                transaction.Rollback();
            }


            return sqldatareader;
        }
    }
}