using Microsoft.EntityFrameworkCore;
using System;

namespace ModelCompany
{
    public class CompanyContext : DbContext
    {
        public CompanyContext()
            : base()
        {
        }
        public CompanyContext(DbContextOptions<CompanyContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //string cs = @"Server=(localdb)\mssqllocaldb;Database=CompanyInfoDB;Integrated Security=True";
            //string cs = @"Server=localhost\SQLEXPRESS2019;Database=XjpStreetDB;Uid=sa;Password=sa;Integrated Security=false";
            //optionsBuilder.UseSqlServer(cs);
        }


        //企业基本信息
        public DbSet<CompanyBasicInfo> CompanyBasicInfo { get; set; }

        //企业税收信息
        public DbSet<CompanyTax> CompanyTax { get; set; }
        //企业享受政策项目信息
        public DbSet<CompanyBuildings> CompanyBuildings { get; set; }
        //企业其他信息
        public DbSet<CompanyOtherInfo> CompanyOtherInfo { get; set; }


        public DbSet<CompanyRoom> CompanyRoom { get; set; }


        //权限
        //public DbSet<User> Users { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RoleUser> RoleUsers { get; set; }
    }
}
