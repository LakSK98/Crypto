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
    public class EventController : ControllerBase
    {
        private readonly crypto_web_app_dbContext db;

        public EventController(crypto_web_app_dbContext context)
        {
            db = context;
        }
        
        // GET: Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> Index()
        {
            return await db.Events.ToListAsync();
        }
    }
}