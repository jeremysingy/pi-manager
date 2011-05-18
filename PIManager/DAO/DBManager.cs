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
        /// Connection string to access the database as a professor
        /// </summary>
        public static readonly string CONNECTION_STRING_PROFESSOR = ConfigurationManager.ConnectionStrings["PIProfConnection"].ToString();

        /// <summary>
        /// Connection string to access the database as a student
        /// </summary>
        public static readonly string CONNECTION_STRING_STUDENT = ConfigurationManager.ConnectionStrings["PIStudConnection"].ToString();

        /// <summary>
        /// Get acces to the unique logger instance
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DBManager));

        /// <summary>
        /// Get the logger for this class
        /// </summary>
        /// <returns>The logger of this class</returns>
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
        /// Perform a select query returning a scalar value
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="paramList">List of parameters to put for this query</param>
        /// <returns>The object resulting of this query</returns>
        public object doSelectScalar(string query, SqlConnection connection, SqlTransaction transaction, List<SqlParameter> paramList)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var param in paramList)
                command.Parameters.Add(param);

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
    }
}