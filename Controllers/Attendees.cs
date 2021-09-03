using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trademarkia.DBContexts;
using Trademarkia.Models;


namespace Trademarkia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeesController : ControllerBase
    {
       
        private MyDBContext myDbContext;

        public AttendeesController(MyDBContext context)
        {
            myDbContext = context;
        }

        [HttpGet]
        public IList<Attendees> Get()
        {
            return (this.myDbContext.Attendees.ToList());
        }

        [HttpPost]
        public void Post([FromBody] Attendees details)
        {

            myDbContext.Attendees.Add(details);

            myDbContext.SaveChanges();
        }
        [HttpDelete]

        [HttpDelete("{id}")]
        public async Task<ActionResult<Attendees>> DeleteAttendee(int id)
        {
            var attendee = await myDbContext.Attendees.FindAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }
            myDbContext.Attendees.Remove(attendee);
            await myDbContext.SaveChangesAsync();

            return attendee;
        }

    }
}
