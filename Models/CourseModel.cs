using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Common;
using System;

namespace CourseNamespace
{
    public class CourseModel
    {
        public string CourseTitle { get; set; }
        public int CourseID { get; set; }
        public string CourseEmail { get; set; }
    
        public static void insertCourse(string MySqlConn, string course, string email)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO user_course_table (course, email)"+
            " VALUES (@course, @email)";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@course",
                    DbType = DbType.String,
                    Value = course,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void updateCourse(string MySqlConn, string course, string email, int course_id)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE user_course_table SET course = @course"+
                " WHERE email = @email"+
                " AND course_id = @course_id";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@course",
                    DbType = DbType.String,
                    Value = course,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@course_id",
                    DbType = DbType.Int32,
                    Value = course_id,
                });

            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public static void removeCourse(string MySqlConn, int course_id, string course, string email)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM user_course_table"+
                " WHERE course_id = @course_id"+
                " AND course = @course"+
                " AND email = @email";
                
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@course_id",
                    DbType = DbType.Int32,
                    Value = course_id,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@course",
                    DbType = DbType.String,
                    Value = course,
                });

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static List<CourseModel> getAllCourses(string MySqlConn, string email)
        {
            List<CourseModel> Courses = new List<CourseModel>();
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM user_course_table "+
                "WHERE email = @email";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {   
                    CourseModel course = new CourseModel();
                    course.CourseID = (int) reader["course_id"];
                    course.CourseTitle = (string) reader["course"];
                    course.CourseEmail = (string) reader["email"];
                    Courses.Add(course);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            conn.Close();

            return Courses;
        } 

    }
}