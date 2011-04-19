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

        public SqlDataReader getProjectList()
        {
            using (SqlConnection connection = new SqlConnection(DB_CONNECTION))
            {
                connection.Open();

                // Because the comparison of XML content, we have to devide this transaction into two projections.
                // First one is getting the projects that are still free.
                // Second one is getting information of projects.
                SqlCommand projectFreeCommand = new SqlCommand(null, connection);
                projectFreeCommand.CommandText = "SELECT pk_project, nbStudent FROM Project GROUP BY pk_project, nbStudent HAVING (SELECT count(*) FROM Person WHERE fk_project = pk_project) < nbStudent;";
                projectFreeCommand.ExecuteReader();

                SqlCommand projectInfoCommand = new SqlCommand(null, connection);
                projectInfoCommand.CommandText = "SELECT description_XML FROM Project WHERE";

                /*SqlCommand command = new SqlCommand(null, connection);
                command.CommandText = "SELECT pk_project, description_xml from Project having count(select * from Person where fk_project = pk_project) < nbStudent;";
                command.Parameters.Add("@id", 20); // for example...
                command.Prepare();*/
                // select pk_project, nbStudent from project group by pk_project, nbStudent having ((select count(*) from Person where fk_project = pk_project) < nbStudent);
                //command.execute();
            }
            return null;
        }

        public void getProjects()
        {

        }

        public SqlDataReader getProject(int id)
        {
            using (SqlConnection connection = new SqlConnection(DB_CONNECTION))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Projects WHERE id='" + id + "'");
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                reader.Read();

                return reader;
            }
        }

        public void addProject(string name, string desc, int nStudents)
        {

        }

        public void openRegistration()
        {

        }
    }
}