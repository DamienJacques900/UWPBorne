using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CafeSuspenduContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Charity> Charities { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }
        public DbSet<UserCafe> UserCafes { get; set; }
        public DbSet<UserPerson> UserPersons { get; set; }

        public CafeSuspenduContext()
            : base("name=CafeSuspenduConnection")
        {

        }
    }
}