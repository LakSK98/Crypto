using Crypto.Data;
using Crypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly crypto_web_app_dbContext db;

        public QuestionController(crypto_web_app_dbContext context)
        {
            db = context;
        }

        // GET: Forum
        [HttpGet]
        public async Task<ActionResult> Index()
        {
	    var questions = await db.Questions.Select(q=>new {id=q.Id,question=q.QuestionDescription,userName=q.User.FirstName+" "+q.User.LastName,date=q.Date}).ToListAsync();
            return Ok(questions);
        }


       
        [HttpPost("newQuestion")]
        public async Task<ActionResult> CreateQuestion(Question question)
        {
            try
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(question);
        }

	[HttpPut("editQuestion")]
        public async Task<ActionResult> EditQuestion(Question question)
        {
            db.Entry(question).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
		return Ok("Saved Successfull.");
            }
            catch (DbUpdateConcurrencyException)
            {
                    throw;
            }

            return NoContent();
        }
    }
}