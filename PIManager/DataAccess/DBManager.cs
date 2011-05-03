using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

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
            

            
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            
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

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@PERSON", SqlDbType.Int).Value = idPerson;
            connection.Open();

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
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            
            // starts local transaction
            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable, "inscriptionProject");
            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
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
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
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
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "UPDATE Person SET pk_project = NULL WHERE pk_person = " + pk_person;

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            
            int affected = command.ExecuteNonQuery();
            
            connection.Close();

            return affected == 1;
        }

        public SqlDataReader getProjects()
        {
            //using (SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING))
            //using (SqlConnection connection = new SqlConnection("Server=160.98.60.31\\SQLPIMANAGER; Database=PIManager; User ID=sa; password=pipass.2011"))
            //{
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.query('data(//title)') AS title, " +
                            "description_xml.query('data(//abreviation)') AS abreviation, " +
                            "description_xml.query('data(//student)') AS nbstudents " +
                            "FROM pimanager.dbo.Project";

            SqlCommand command = new SqlCommand(query/*"SELECT * FROM Project WHERE pk_period IS NULL"*/, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
            //}
        }

        public SqlDataReader getProject(int id)
        {
            /*using (SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Projects WHERE id='" + id + "'");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                reader.Read();

                return reader;
            }*/
            return null;
        }

        public void addProject(string name, string desc, int nStudents)
        {

        }

        public void openRegistration()
        {

        }
    }
}