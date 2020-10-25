using Microsoft.EntityFrameworkCore;
using RESTServer.Entities;
using RESTServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTServer.Data
{
    public class EventDBContext : DbContext
    {
        public EventDBContext (DbContextOptions<EventDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BadEvent> Events { get; set; }
        public DbSet<Severity> Severity { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
