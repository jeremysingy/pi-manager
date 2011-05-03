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
        /// Gets a list of projects opened for the inscription.
        /// </summary>
        /// <returns></returns>
        public SqlDataReader getProjectList()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            connection.Open();

            // Gets the list of projects that are opened for inscription and still free.
            SqlCommand projectFreeCommand = new SqlCommand(null, connection);
            projectFreeCommand.CommandText = "SELECT pk_project, description_XML, nbStudent FROM Project WHERE nbStudent > (SELECT COUNT(*) FROM Person WHERE fk_project = pk_project);";

            SqlDataReader dataReader = projectFreeCommand.ExecuteReader();

            //connection.Close();
            
            return dataReader;
        }

        public SqlDataReader getProjects()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.query('data(//title)') AS title, " +
                            "description_xml.query('data(//abreviation)') AS abreviation, " +
                            "description_xml.query('data(//student)') AS nbstudents " +
                            "FROM pimanager.dbo.Project";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader getProject(int id)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.query('data(//title)') AS title, " +
                            "description_xml.query('data(//abreviation)') AS abreviation, " +
                            "description_xml.query('data(//student)') AS nbstudents " +
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
    }
}