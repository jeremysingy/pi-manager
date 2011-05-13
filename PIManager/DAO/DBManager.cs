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

namespace PIManager.DAO
{
    /// <summary>
    /// Manages access to the database
    /// </summary>
    public class DBManager
    {
        /// <summary>
        /// Connection string to access the database
        /// </summary>
        public static readonly string DB_CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PIDBConnection"].ToString();

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
            return new SqlConnection(DB_CONNECTION_STRING);
        }

        /// <summary>
        /// Perform a select query
        /// </summary>
        public SqlDataReader doSelect(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
            
            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteReader();
        }

        /// <summary>
        /// Perform a select query returning a scalar value.
        /// </summary>
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// <returns></returns>
        public object doSelectScalar(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            return command.ExecuteScalar();
        }
        
        /// <summary>
        /// Perform an update query
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// </summary>
        public int doUpdate(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Perform a delete query
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// </summary>
        public int doDelete(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Perform an insert query
        /// <param name="query">Query to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="param">Parameters to put for this query</param>
        /// </summary>
        public int doInsert(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            query += " SET @newId = SCOPE_IDENTITY()";
            //SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            //SqlTransaction transaction = connection.BeginTransaction(isolationLevel);

            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            SqlParameter idParam = new SqlParameter("@newId", SqlDbType.Int);
            idParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(idParam);

            //SqlParameterCollection s = new SqlParameterCollection();

            command.ExecuteNonQuery();

            //transaction.Commit();
            return (int)idParam.Value;
        }

        /// <summary>
        /// Execute a stored procedure
        /// <param name="name">Name of the stored procedure to execute</param>
        /// <param name="connection">Connection to use</param>
        /// <param name="transaction">Transaction to use</param>
        /// <param name="paramList">Parameters to put for this stored procedure</param>
        /// </summary>
        public void executeProcedure(string name, SqlConnection connection, SqlTransaction transaction, List<SqlParameter> paramList)
        {
            SqlCommand command = new SqlCommand(name, connection, transaction);
            command.CommandType = CommandType.StoredProcedure;

            foreach (var param in paramList)
                command.Parameters.Add(param);

            command.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        // TODO: replace these methods in DAO!
        //////////////////////////////////////////////////////////////////////////////////////////

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
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error while getting person: " + exception.Message);
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
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting person type: " + exception.Message);
            }

            transaction.Commit();
            connection.Close();


            return person_type;
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
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting project inscriptions: " + exception.Message);
            }


            return sqldatareader;
        }
		
		public SqlDataReader getFullProjects()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showProjects");

            string query = "SELECT pr.pk_project, description_xml.value('(//title)[1]', 'varchar(80)') AS title, pe.pk_person FROM project pr, person pe, period time WHERE pr.pk_person = pe.pk_person AND pr.pk_period = time.pk_period AND date_close < getdate()";

            SqlDataReader sqldatareader = null;

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                sqldatareader = command.ExecuteReader(CommandBehavior.CloseConnection);


            }
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting project inscriptions: " + exception.Message);
            }


            return sqldatareader;
        }

        public SqlDataReader getPersonName(int pk_person)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "showPeronName");

            string query = "SELECT firstname, lastname FROM person  WHERE pk_person = @pk_person";

            SqlDataReader sqldatareader = null;

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@pk_person", pk_person);

                sqldatareader = command.ExecuteReader(CommandBehavior.CloseConnection);


            }
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting project inscriptions: " + exception.Message);
            }


            return sqldatareader;
        }

        public SqlDataReader getProjectGroup(int pk_project)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getProjectGroup");

            string query = "SELECT firstname, lastname FROM person  WHERE pk_project = @pk_project";

            SqlDataReader sqldatareader = null;

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@pk_project", pk_project);

                sqldatareader = command.ExecuteReader(CommandBehavior.CloseConnection);


            }
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting project inscriptions: " + exception.Message);
            }


            return sqldatareader;
        }


        public SqlDataReader getTechnologyProject(int pk_project)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted, "getTechnologyProject");

            string query = "SELECT name FROM project_techno prt, technology t  WHERE prt.pk_project = @pk_project AND prt.pk_techno = t.pk_technology";

            SqlDataReader sqldatareader = null;

            try
            {

                SqlCommand command = new SqlCommand(query, connection, transaction);

                command.Parameters.AddWithValue("@pk_project", pk_project);

                sqldatareader = command.ExecuteReader(CommandBehavior.CloseConnection);


            }
            catch (SqlException exception)
            {
                transaction.Rollback();
                log.Error("Error getting project inscriptions: " + exception.Message);
            }


            return sqldatareader;
        }
    }
}