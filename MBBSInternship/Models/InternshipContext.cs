using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class InternshipContext : DbContext
    {
        public InternshipContext(DbContextOptions<InternshipContext> options)
            : base(options)
        { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<District> Districts { get; set; }
    }

}
