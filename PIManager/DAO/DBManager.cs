using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;
using log4net;
using PIManager.Models;
using PIManager.Login;
using System.Web.Security;

namespace PIManager.DAO
{
    /// <summary>
    /// Manages access to the database
    /// </summary>
    public class DBManager
    {
        /// <summary>
        /// Current user logged in to the applicaiton
        /// </summary>
        //MemberShipPIUser myConnectedUser;

        /// <summary>
        /// Connection string to access the database
        /// </summary>
        public static readonly string CONNECTION_STRING_PROFESSOR = ConfigurationManager.ConnectionStrings["PIProfConnection"].ToString();
        public static readonly string CONNECTION_STRING_STUDENT   = ConfigurationManager.ConnectionStrings["PIStudConnection"].ToString();

        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DBManager));

        public static ILog getLog() {
            return log;
        }

        /// <summary>
        /// Create a new connection to the database
        /// </summary>
        public SqlConnection newConnection()
        {
            if (Roles.IsUserInRole("professor"))
                return new SqlConnection(CONNECTION_STRING_PROFESSOR);
            else
                return new SqlConnection(CONNECTION_STRING_STUDENT);
        }

        /// <summary>
        /// Perform a select query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// <returns>The SqlDataReader to read to result of this query</returns>
        public SqlDataReader doSelect(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
            
            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteReader();
        }

        /// <summary>
        /// Perform a select query returning a scalar value
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// <returns>The object resulting of this query</returns>
        public object doSelectScalar(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            return command.ExecuteScalar();
        }
        
        /// <summary>
        /// Perform an update query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// <returns>The number of rows affected by the query</returns>
        public int doUpdate(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

        /*public int doUpdate(string query, SqlConnection connection, SqlTransaction transaction, List<SqlParameter> paramList)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var param in paramList)
                command.Parameters.Add(param);

            return command.ExecuteNonQuery();
        }*/

        /// <summary>
        /// Perform a delete query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        public int doDelete(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Perform an insert query
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        public int doInsert(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            query += " SET @newId = SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            SqlParameter idParam = new SqlParameter("@newId", SqlDbType.Int);
            idParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(idParam);

            command.ExecuteNonQuery();

            return (int)idParam.Value;
        }

        /// <summary>
        /// Execute a stored procedure
        /// </summary>
        /// <param name="name">Name of the stored procedure to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="paramList">Parameters to put for this stored procedure</param>
        public void executeProcedure(string name, SqlConnection connection, SqlTransaction transaction, List<SqlParameter> paramList)
        {
            SqlCommand command = new SqlCommand(name, connection, transaction);
            command.CommandType = CommandType.StoredProcedure;

            foreach (var param in paramList)
                command.Parameters.Add(param);

            command.ExecuteNonQuery();
        }


        public int getPersonType(int pk_person)
        {
            int person_type = -1;
            string query = "SELECT role FROM Person WHERE pk_person = @pk_person;";

            SqlConnection connection = new SqlConnection(CONNECTION_STRING_PROFESSOR); 
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
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting person type: " + exception.Message);
            }

            transaction.Commit();
            connection.Close();


            return person_type;
        }
    }
}