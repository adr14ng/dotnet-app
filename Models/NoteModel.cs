using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace NoteNamespace
{
    public class NoteModel
    {
        public string NoteTitle { get; set; }
        public int NoteID { get; set; }
        public string NoteText { get; set; }
        public DateTime NoteDate { get; set; }
        public string NoteCourse { get; set; }
        public string NoteEmail { get; set; }
 
        
        public static void insertNote(string MySqlConn, string course, string email, string note_title, string note)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO user_note_table (course, email, note_title, note)"+
            " VALUES (@course, @email, @note_title, @note)";
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
                    ParameterName = "@note_title",
                    DbType = DbType.String,
                    Value = note_title,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note",
                    DbType = DbType.String,
                    Value = note,
                });
            
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        
        // string queryUpdateCourseNotes = "UPDATE user_note_table "+
        //     "SET course='"+ course.CourseTitle + 
        //     "' WHERE email='"+ email + 
        //     "' AND course='"+ course_name +"';"; 


        public static void updateNoteCourse(string MySqlConn, string course, string email, string course_name)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE user_note_table"+
                " SET course = @course"+
                " WHERE email = @email"+
                " AND course = @course_name";
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
                    ParameterName = "@course_name",
                    DbType = DbType.String,
                    Value = course_name,
                });

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void updateNote(string MySqlConn, string note_title, string note_text, string email, int note_id)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE user_note_table"+
                " SET note_title = @note_title"+
                ", note = @note_text"+
                " WHERE email = @email"+
                " AND note_id = @note_id";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note_title",
                    DbType = DbType.String,
                    Value = note_title,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note_text",
                    DbType = DbType.String,
                    Value = note_text,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note_id",
                    DbType = DbType.Int32,
                    Value = note_id,
                });

            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void deleteNoteByCourse(string MySqlConn, string course, string email)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM user_note_table"+
                " WHERE course = @course"+
                " AND email = @email";
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

        public static void deleteNoteByID(string MySqlConn, int note_id, string email)
        {
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM user_note_table"+
                " WHERE note_id = @note_id"+
                " AND email = @email";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note_id",
                    DbType = DbType.Int32,
                    Value = note_id,
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

            // string queryCourseNotes = "SELECT note_id, note_title FROM user_note_table "+
            // "WHERE email='"+ email + 
            // "' AND  course='" + course + "';";

        public static List<NoteModel> getCourseNotes(string MySqlConn, string email, string course)
        {
            List<NoteModel> Notes = new List<NoteModel>();
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM user_note_table "+
                "WHERE email = @email"+
                " AND course = @course";
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
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {   
                    NoteModel note = new NoteModel();
                    note.NoteID = (int) reader["note_id"];
                    note.NoteCourse = (string) reader["course"];
                    note.NoteEmail = (string) reader["email"];
                    note.NoteTitle = (string) reader["note_title"];
                    note.NoteText = (string) reader["note"];
                    note.NoteDate = (DateTime) reader["date"];
                    Notes.Add(note);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            conn.Close();

            return Notes;
        }

            // string queryNote = "SELECT * FROM user_note_table "+
            // "WHERE email='"+ email + 
            // "' AND  note_id='" + note_id + "';";

        public static List<NoteModel> getNoteByID(string MySqlConn, string email, int note_id)
        {
            List<NoteModel> Notes = new List<NoteModel>();
            MySqlConnection conn = new MySqlConnection(MySqlConn);
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM user_note_table "+
                "WHERE email = @email"+
                " AND note_id = @note_id";
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@email",
                    DbType = DbType.String,
                    Value = email,
                });
            cmd.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@note_id",
                    DbType = DbType.Int32,
                    Value = note_id,
                });
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while(reader.Read())
                {   
                    NoteModel note = new NoteModel();
                    note.NoteID = (int) reader["note_id"];
                    note.NoteCourse = (string) reader["course"];
                    note.NoteEmail = (string) reader["email"];
                    note.NoteTitle = (string) reader["note_title"];
                    note.NoteText = (string) reader["note"];
                    note.NoteDate = (DateTime) reader["date"];
                    Notes.Add(note);
                }
            }
            else
            {
                Console.WriteLine("No rows found.");
            }
            reader.Close();
            conn.Close();

            return Notes;
        }


    }
}