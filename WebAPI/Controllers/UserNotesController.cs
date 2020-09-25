using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebAPI.Database;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserNotesController : ControllerBase
    {
        private ApplicationDatabase _db;
        public UserNotesController(ApplicationDatabase db)
        {
            _db = db;
        }

        //Get
        [HttpGet("getallnotes")]
        public ICollection<UserNote> GetAllNotes()
        {
            int UserID = GetUserIDFromClaims();
            ICollection<UserNote> Notes = _db.User
                .Include(u => u.UserNote)
                .First(u => u.UserID == UserID)
                .UserNote;
            return Notes;
        }
        [HttpPut("updatenote")]
        public object PostUpateNote([FromBody] UserNote Body)
        {
            int UserID = GetUserIDFromClaims();
            if (ModelState.IsValid)
            {
                var Note = _db.User
                    .Include(u => u.UserNote)
                    .First(u => u.UserID == UserID)
                    .UserNote
                    .Single(n => n.NoteID == Body.NoteID);
               
                Note.Heading = Body.Heading;
                Note.Note = Body.Note;
                
                _db.SaveChanges();
                return new { message = "successful" };
            }
            else
            {
                return new { message = "Invalid Request" };
            }
        }

        [HttpPost("newnote")]
        public void CreateNote()
        {
            int UserID = GetUserIDFromClaims();
            UserNote NewNote = new UserNote {
                Heading = "<h1>New Note</h1>",
                Note = "<p> </p>"
            };

            var User = _db.User.Include(u => u.UserNote).Single(u => u.UserID == UserID);
            User.UserNote.Add(NewNote);
            _db.SaveChanges();
        }

        [HttpDelete("Delete/{id}")]
        public void DeleteNote([FromRoute] int ID)
        {
            int UserID = GetUserIDFromClaims();
            var User = _db.User
                .Include(u => u.UserNote)
                .Single(u => u.UserID == UserID);
            var Note = _db.UserNote
                .Single(n => n.NoteID == ID);
            //This breaks the realtion
            User.UserNote.Remove(Note);
            //This removes it from the table
            _db.UserNote.Remove(Note);
            _db.SaveChanges();
        }
        
        private int GetUserIDFromClaims()
        {
            ClaimsPrincipal CurrentUser = HttpContext.User;
            int UserID;
            UserID = int.Parse(CurrentUser
                .Claims
                .First(c => c.Type == JwtRegisteredClaimNames.Sid)
                .Value);
            return UserID;
        }
    }
}
