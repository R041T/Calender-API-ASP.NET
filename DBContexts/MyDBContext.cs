using Trademarkia.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trademarkia.DBContexts
{
    public class MyDBContext : DbContext
    {

        public DbSet<Calendar> Calendar { get; set; }
        public DbSet<Attendees> Attendees { get; set; }


        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  

            modelBuilder.Entity<Calendar>().ToTable("calendar");
            modelBuilder.Entity<Attendees>().ToTable("attendees");


            // Configure Primary Keys  

            modelBuilder.Entity<Calendar>().HasKey(u => u.eventid).HasName("PK_eventid");
            modelBuilder.Entity<Attendees>().HasKey(u => u.attendeeid).HasName("PK_attendeeid");


            // Configure indexes  



        }

        internal object GetData(int id)
        {
            throw new NotImplementedException();
        }
    }
}