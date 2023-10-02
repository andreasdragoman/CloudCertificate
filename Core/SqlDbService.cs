using Microsoft.Data.SqlClient;
using SBShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SqlDbService
    {
        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> persons = new List<PersonModel>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "andreas-sql-db.database.windows.net";
                builder.UserID = "andreas";
                builder.Password = "and#reas1";
                builder.InitialCatalog = "AndreasSqlDb";

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = "SELECT firstname, lastname FROM Persons";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                persons.Add(new PersonModel
                                {
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString()
                                });
                                //Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                            }
                        }
                        connection.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            return persons;
        }
    }
}
