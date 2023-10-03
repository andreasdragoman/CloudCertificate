using Microsoft.Data.SqlClient;
using SBShared.Models;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Options;

namespace AzureSQL
{
    public class SqlDbService : ISqlDbService
    {
        private readonly SQLSettings _settings;

        public SqlDbService(IOptions<SQLSettings> settings)
        {
            _settings = settings.Value;
        }

        public List<PostModel> GetAllPosts()
        {
            List<PostModel> posts = new List<PostModel>();
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = _settings.DataSource;
                builder.UserID = _settings.UserId;
                builder.Password = _settings.Password;
                builder.InitialCatalog = _settings.InitialCatalog;

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    String sql = "SELECT id, name, description, personId FROM Posts";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                posts.Add(new PostModel
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Description = reader["description"].ToString(),
                                    PersonId = reader["personId"].ToString()
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
            return posts;
        }
    }
}
