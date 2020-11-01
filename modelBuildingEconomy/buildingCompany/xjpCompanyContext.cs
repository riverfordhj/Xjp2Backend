using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class xjpCompanyContext : DbContext
    {
        public xjpCompanyContext()
            : base()
        {
        }

        public xjpCompanyContext(DbContextOptions<xjpCompanyContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["XjpDatabase"].ConnectionString);
            //string cs1 = ConfigurationManager.ConnectionStrings["XjpDatabase"].ConnectionString;
            //string cs = @"Server=(localdb)\mssqllocaldb;Database=BuildingCompanyDB;Integrated Security=True";
            //string cs = @"Server=localhost\SQLEXPRESS2019;Database=BuildingCompanyDB;Uid=sa;Password=sa;Integrated Security=false";//

            //optionsBuilder.UseSqlServer(cs);
        }

        //楼宇
        public DbSet<CompanyBuilding> CompanyBuilding { get; set; }

        //公司信息
        public DbSet<Company> Company { get; set; }

        public DbSet<CompanyEconomy> CompanyEconomy { get; set; }
        public DbSet<CompanyOtherInfo> CompanyOtherInfo { get; set; }
        public DbSet<BuildingFloor> BuildingFloor { get; set; }
        public DbSet<CompanyTaxInfo> CompanyTaxInfo { get; set; }

    }
}