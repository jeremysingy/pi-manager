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
    /// Gets information about technologies from the database in the model form.
    /// This is a Data Access Object (DAO)
    /// </summary>
    public class TechnologyAccess
    {
        /// <summary>
        /// The manager used to connect to the database
        /// </summary>
        private DBManager myDBManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public TechnologyAccess()
        {
            myDBManager = new DBManager();
        }

        /// <summary>
        /// Get all the technologies stored in the database
        /// </summary>
        /// <returns>A dictionnary indexed by the technology ids</returns>
        public Dictionary<int, Technology> getTechnologies()
        {
            string query = "SELECT * FROM Technology";
            Dictionary<int, Technology> list = new Dictionary<int, Technology>();

            using (SqlConnection connection = myDBManager.newConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

                SqlDataReader reader = myDBManager.doSelect(query, connection, transaction, new Dictionary<string, object>());

                while (reader.Read())
                {
                    int id = (int)reader["pk_technology"];
                    string name = (string)reader["name"];

                    list.Add(id, new Technology(id, name));
                }

                reader.Close();
                transaction.Commit();
            }

            return list;
        }
    }
}