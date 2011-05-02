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
        public readonly string DB_CONNECTION = ConfigurationManager.ConnectionStrings["PIDBConnection"].ToString();
        

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
            SqlConnection connection = new SqlConnection(DB_CONNECTION);
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
            using (SqlConnection connection = new SqlConnection(DB_CONNECTION))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Project WHERE pk_period IS NULL");

                return command.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }

        public SqlDataReader getProject(int id)
        {
            /*using (SqlConnection connection = new SqlConnection(DB_CONNECTION))
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