using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

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

        public SqlConnection newConnection()
        {
            return new SqlConnection(DB_CONNECTION_STRING);
        }

        public SqlDataReader doSelect(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);
            
            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteReader();
        }

        public object doSelectScalar(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            return command.ExecuteScalar();
        }

        public int doUpdate(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

        public int doDelete(string query, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(query, connection, transaction);

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            return command.ExecuteNonQuery();
        }

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

        public void executeProcedure(string name, SqlConnection connection, SqlTransaction transaction, Dictionary<string, object> param)
        {
            SqlCommand command = new SqlCommand(name, connection, transaction);
            command.CommandType = CommandType.StoredProcedure;

            foreach (var pair in param)
                command.Parameters.AddWithValue(pair.Key, pair.Value);

            command.ExecuteNonQuery();
        }

        //////////////////////////////////////////////////////////////////////////////////////////

        public SqlDataReader getProjects()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT pk_project, " +
                            "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                            "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                            "description_xml.query('//description') AS description, " +
                            "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                            "pk_person " +
                            "FROM pimanager.dbo.Project";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /*public SqlDataReader getProject(int id)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);

            string query = "SELECT pk_project, " +
                            "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                            "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                            "description_xml.query('//description') AS description, " +
                            "description_xml.value('(//student)[1]', 'int') AS nbstudents, " +
                            "pk_person" +
                            "FROM pimanager.dbo.Project " +
                            "WHERE pk_project = @id";

            SqlCommand command = new SqlCommand(query, connection, transaction);
            command.Parameters.AddWithValue("@id", id);

            string queryTechnos = "SELECT * FROM Project_Techno WHERE pk_project = @id";
            SqlCommand commandTechnos = new SqlCommand(queryTechnos, connection, transaction);
            command.Parameters.AddWithValue("@id", id);

            SqlDataReader 

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }*/

        public void addProject(string name, string desc, int nStudents)
        {

        }

        public bool modifyProject(int id, Project oldProject, Project newProject)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            string selectQuery = "SELECT pk_project, " +
                "description_xml.value('(//title)[1]', 'varchar(80)') AS title, " +
                "description_xml.value('(//abreviation)[1]', 'varchar(50)') AS abreviation, " +
                "description_xml.query('//description') AS description, " +
                "description_xml.value('(//student)[1]', 'int') AS nbstudents " +
                "FROM pimanager.dbo.Project " +
                "WHERE pk_project = @id";

            SqlCommand selectCommand = new SqlCommand(selectQuery, connection, transaction);
            selectCommand.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = selectCommand.ExecuteReader();
            string name = (string)reader["title"];
            string abreviation = (string)reader["abreviation"];
            string desc = (string)reader["description"];
            int nbStudents = (int)reader["nbstudents"];
            Project crtProject = new Project(name, abreviation, desc, nbStudents);

            if(!oldProject.isEquivalent(crtProject))
            {
                reader.Close();
                return false;
            }

            string updateQuery = "UPDATE pk_project, " +
                "SET description_xml.modify('replace value of (//title[1]/text() with @title, " +
                "description_xml.modify('replace value of (//abreviation[1]/text() with @abreviation, " +
                "description_xml.modify('replace value of //description[1] with @description, " +
                "description_xml.modify('replace value of //student[1]/text() with @nbstudents " +
                "WHERE pk_project = @id";

            SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction);
            updateCommand.Parameters.AddWithValue("@id", id);
            updateCommand.Parameters.AddWithValue("@title", newProject.Name);
            updateCommand.Parameters.AddWithValue("@abreviation", newProject.Abreviation);
            updateCommand.Parameters.AddWithValue("@description", newProject.Description);
            updateCommand.Parameters.AddWithValue("@nbstudents", newProject.NbStudents);

            updateCommand.ExecuteNonQuery();
            transaction.Commit();

            return true;
        }

        public void openRegistration()
        {

        }

        private SqlDataReader selectQuery(string query, SqlTransaction transaction)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);
            SqlCommand command = new SqlCommand(query, connection, transaction);

            command.Connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

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
            catch (SqlException sqlError)
            {
                transaction.Rollback();
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
            catch (SqlException sqlError)
            {
                transaction.Rollback();
            }

            

            transaction.Commit();

            connection.Close();


            return person_type;

        }

        public SqlDataReader getTechnologies()
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT * FROM Technology";

            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public SqlDataReader getPersons(int role)
        {
            SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING);

            string query = "SELECT * FROM Person WHERE role = @role";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@role", role);

            connection.Open();

            return command.ExecuteReader(CommandBehavior.CloseConnection);
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
            catch (SqlException sqlError)
            {
                transaction.Rollback();
            }


            return sqldatareader;
        }
    }
}