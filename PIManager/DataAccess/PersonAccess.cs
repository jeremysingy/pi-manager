using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PIManager.DataAccess
{
    public class PersonAccess
    {
        private DBManager myDBManager;

        public PersonAccess()
        {
            myDBManager = new DBManager();
        }

        public List<Person> getPersons(int role)
        {
            List<Person> list = new List<Person>();
            SqlDataReader reader = myDBManager.getPersons(role);

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

            return list;
        }
    }
}