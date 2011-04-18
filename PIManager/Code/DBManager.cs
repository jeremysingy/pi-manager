using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PIManager.Code
{
    public class DBManager
    {
        static const string DB_CONNECTION = ConfigurationManager.ConnectionStrings["PIDBConnection"].ToString();


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

        public void getProjects()
        {

        }

        public void addProject(string name, string desc, int nStudents)
        {

        }

        public void openRegistration()
        {

        }
    }
}