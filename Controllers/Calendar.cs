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
    public class CalendarController : ControllerBase
    {
       
        private MyDBContext myDbContext;

        public CalendarController(MyDBContext context)
        {
            myDbContext = context;
        }

        [HttpGet]
        public IQueryable<Object> Get()
        {
            //return (this.myDbContext.Operations.ToList());

            return (from c in myDbContext.Calendar
                    join a in myDbContext.Attendees
                    on c.eventid equals a.eventid

                    select new
                    {
                        eventId = c.eventid,
                        eventTitle = c.eventtitle,
                        startDate = c.startdate,
                        endDate = c.enddate,
                        eventTime = c.eventtime,
                        attendeeid=a.attendeeid,
                        attendeeName = a.attendeename,
                    });
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Calendar details)
        {
            Console.WriteLine(id);
            Calendar result = (from c in myDbContext.Calendar
                                 where c.eventid == id
                                 select c).SingleOrDefault();

            result.eventtitle = details.eventtitle;
            result.startdate = details.startdate;
            result.enddate = details.enddate;
            result.eventtime = details.eventtime;

            myDbContext.SaveChanges();

        }

        [HttpPost]

        public void Post([FromBody] Calendar details)
        {
           
            myDbContext.Calendar.Add(details);
            
            myDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Calendar>> DeleteCalendar(int id)
        {
            var calevent = await myDbContext.Calendar.FindAsync(id);
            if (calevent == null)
            {
                return NotFound();
            }
            myDbContext.Calendar.Remove(calevent);
            await myDbContext.SaveChangesAsync();

            return calevent;
        }
    }
}
