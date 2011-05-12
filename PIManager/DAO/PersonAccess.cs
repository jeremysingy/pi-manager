using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using PIManager.Models;
using System.Data;

namespace PIManager.DAO
{
    /// <summary>
    /// Gets information about a person from the database in the model form.
    /// This is a Data Access Object (DAO)
    /// </summary>
    public class PersonAccess
    {
        /// <summary>
        /// The manager used to connect to the database
        /// </summary>
        private DBManager myDBManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public PersonAccess()
        {
            myDBManager = new DBManager();
        }

        /// <summary>
        /// Get all person of the given role from the database
        /// </summary>
        /// <param name="role">Role of the person. Sudent is 1 and professor is 2</param>
        /// <returns></returns>
        public List<Person> getPersons(int role)
        {
            string query = "SELECT * FROM Person WHERE role = @role";
            List<Person> list = new List<Person>();
            
            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                Dictionary<string, object> param = new Dictionary<string, object>();
                param.Add("@role", role);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, param);

                while (reader.Read())
                {
                    int id = (int)reader["pk_person"];
                    string lastName = (string)reader["lastname"];
                    string firstName = (string)reader["firstname"];
                    string email = (string)reader["email"];
                    string login = (string)reader["login"];

                    list.Add(new Person(id, lastName, firstName, email, login, role));
                }

                reader.Close();
                transaction.Commit();
            }

            return list;
        }
    }
}