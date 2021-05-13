using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            //string cs1 = ConfigurationManager.ConnectionStrings["XjpDatabase"].ConnectionString;
            //string cs = @"Server=(localdb)\mssqllocaldb;Database=XjpStreetDB;Integrated Security=True";
            //string cs = @"Server=localhost\SQLEXPRESS2019;Database=XjpStreetDB;Uid=sa;Password=sa;Integrated Security=false";
            //HJ - HOME - DESKTOP\SQLEXPRESS2019; Initial Catalog = XjpStreetDB1; User ID = sa; Password = ********; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False
           // optionsBuilder.UseSqlServer(cs);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //设置身份证唯一
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.PersonId)
                .IsUnique();
        }

        //街道单元
        public DbSet<StreetUnit> Streets { get; set; }
        public DbSet<Community> Communitys { get; set; }
        public DbSet<NetGrid> NetGrids { get; set; }
        public DbSet<Subdivision> Subdivisions { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Room> Rooms { get; set; }

        //人员信息
        public DbSet<Person> Persons { get; set; }


        public DbSet<CompanyInfo> CompanyInfos { get; set; }
        public DbSet<OtherInfos> OtherInfos { get; set; }
        public DbSet<PersonRoom> PersonRooms { get; set; }

        public DbSet<PoorPeople> PoorPeoples { get; set; }
        public DbSet<SpecialGroup> SpecialGroups { get; set; }
        public DbSet<MilitaryService> MilitaryService { get; set; }
        public DbSet<Disability> Disability { get; set; }
        

        //权限
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUser> RoleUsers { get; set; }

        public DbSet<PersonHouseData> PersonHouseDatas { get; set; }
    }
}
