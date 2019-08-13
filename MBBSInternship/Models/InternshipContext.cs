using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class InternshipContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public InternshipContext(DbContextOptions<InternshipContext> options)
            : base(options)
        { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PersonHospitalRelationship> PersonHospitalRelationships { get; set; }
        public DbSet<SelectedHospitalLog> SelectedHospitalLogs { get; set; }
        public DbSet<GlobalSettings> GlobalSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Transaction>().ToTable("Transaction");
            //base.OnModelCreating(modelBuilder);
        }
    }

}
