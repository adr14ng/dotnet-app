using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Common;
using System;
using System.Text;


namespace UserNamespace
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public static UserModel getUser(string MySqlConn, string email)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM user_table WHERE email = @email";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            MySqlDataReader reader = cmd.ExecuteReader();
            UserModel user = null;
            if (reader.HasRows)
            {
                user = new UserModel();
                while(reader.Read())
                    {   
                        user.UserID = (int) reader["user_id"];
                        user.Email = (string) reader["email"];
                        user.FirstName = (string) reader["first_name"];
                        user.LastName = (string) reader["last_name"];
                        user.Password = (string) reader["password"];
                    }

            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            conn.Close();

            return user;
        }

        public static void registerUser(string MySqlConn, string email, string first_name, string last_name, string password)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO user_table (email, first_name, last_name, password)"+
            " VALUES(@email, @first_name, @last_name, @password)";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@first_name",
                    DbType = DbType.String,
                    Value = first_name,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@last_name",
                    DbType = DbType.String,
                    Value = last_name,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@password",
                    DbType = DbType.String,
                    Value = password,
                });
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
}