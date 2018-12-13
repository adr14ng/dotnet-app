using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using CourseNamespace;
using NoteNamespace;


namespace webapplication.Controllers
{ 

    [Route("api/[controller]")]
    public class CoursesController : Controller
    {

        string connectionstring;

        public CoursesController (IConfiguration configuration)
        {
            this.connectionstring = configuration.GetConnectionString("DefaultConnection");
        }

        // GET api/values
        [HttpGet,Authorize]
        public IEnumerable<CourseModel> GetCourses()
        {   
            string email = User.Identity.Name;

            return CourseModel.getAllCourses(this.connectionstring, email);;
        }

        // GET

        [HttpGet("{course}"),Authorize]
        public IEnumerable<NoteModel> GetCourseNotes(string course)
        {
            string email = User.Identity.Name;

            return NoteModel.getCourseNotes(this.connectionstring, email, course);
        }

        [HttpGet("{course}/{note_id}"),Authorize]
        public IEnumerable<NoteModel> GetCourseNote(string course, int note_id)
        {
            string email = User.Identity.Name;

            return NoteModel.getNoteByID(this.connectionstring, email, note_id);
        }

        // Add 

        [HttpPost("course/add/new"), Authorize]
        public IActionResult AddCourse([FromBody]CourseModel course)
        {
            string email = User.Identity.Name;

            CourseModel.insertCourse(this.connectionstring, course.CourseTitle, email);

            return Ok();
        }

        [HttpPost("{course}/note/add/new"), Authorize]
        public IActionResult AddNote([FromBody]NoteModel note, string course)
        {
            if (note == null)
            {
                return BadRequest("Invalid client request");
            }

            string email = User.Identity.Name;

            NoteModel.insertNote(this.connectionstring, course, email, note.NoteTitle, note.NoteText);

            return Ok();
            
        }

        //Edit

        [HttpPost("{course_name}/course/edit/{course_id}"), Authorize]
        public IActionResult EditCourse([FromBody]CourseModel course, string course_name, int course_id)
        {
            string email = User.Identity.Name;

            CourseModel.updateCourse(this.connectionstring, course.CourseTitle, email, course_id);

            NoteModel.updateNoteCourse(this.connectionstring, course.CourseTitle, email, course_name);

            return Ok();
        }

        [HttpPost("{course}/note/edit/{note_id}"), Authorize]
        public IActionResult EditNote([FromBody]NoteModel note, string course, int note_id)
        {
            if (note == null)
            {
                return BadRequest("Invalid client request");
            }

            string email = User.Identity.Name;

            NoteModel.updateNote(this.connectionstring, note.NoteTitle, note.NoteText, email, note_id);

            return Ok();
        }

        //Delete

        [HttpDelete("{course_name}/course/delete/{course_id}"), Authorize]
        public IActionResult DeleteCourse(string course_name, int course_id)
        {

            string email = User.Identity.Name;

            CourseModel.removeCourse(this.connectionstring, course_id, course_name, email);

            NoteModel.deleteNoteByCourse(this.connectionstring, course_name, email);

            return Ok();
        }

        [HttpDelete("{course}/note/delete/{note_id}"), Authorize]
        public IActionResult DeleteNote(string course, int note_id)
        {

            string email = User.Identity.Name;

            NoteModel.deleteNoteByID(this.connectionstring, note_id, email);

            return Ok();
        }
    }
}
