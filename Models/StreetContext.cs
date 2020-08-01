using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Models
{
    public class StreetContext : DbContext
    {
        public StreetContext()
            : base()
        {
        }
        public StreetContext(DbContextOptions<StreetContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["XjpDatabase"].ConnectionString);
            string cs = ConfigurationManager.ConnectionStrings["XjpDatabase"].ConnectionString;
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=XjpStreetDB;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置身份证唯一
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PersonId)
                .IsUnique();
        }

        public DbSet<StreetUnit> Streets { get; set; }
        public DbSet<Community> Communitys { get; set; }
        public DbSet<NetGrid> NetGrids { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Person> Persons { get; set; }
    }
}
