using ModelsBuildingEconomy.buildingCompany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModelsBuildingEconomy.DataHelper
{
    public class companyRepository
    {
        private xjpCompanyContext _context;

        public companyRepository(xjpCompanyContext context)
        {
            _context = context;
        }

        public IEnumerable<object> GetbuildingEconomy_company()
        {    
            var data = from companyBD in _context.CompanyBuilding
                       from company in companyBD.Company
                       select new
                       {
                           company,
                           companyBD.BuildingName,
                           companyBD.StreetName,
                           companyEco = company.CompanyEconomy,
                           company_OtherInfo = company.CompanyOtherInfo
                       };


            return data;
        }

        public IQueryable<object> GetCompanysByBuilding(int id)
        {
            var data = from companyBD in _context.CompanyBuilding.Where(b => b.Id == id)
                       from company in companyBD.Company
                       select new
                       {
                           company,
                           companyBD.BuildingName,
                           companyBD.StreetName,
                           companyEco = company.CompanyEconomy,
                           company_OtherInfo = company.CompanyOtherInfo
                       };

            return data;

        }


        public IEnumerable<object> BuildingEcoFields()
        {
            var demoData = from companyBD in _context.CompanyBuilding
                       from company in companyBD.Company
                       select new
                       {
                           companyBD.BuildingName,
                           company.CompanyName,
                           company.Contacts,
                           company.Phone,
                           company.BusinessDirection
                       };

            return demoData;

        }

        public IQueryable<object> GetCompanysByFloor(string buildingName, string floor)
        {
            var comByBuilidng = from companyBD in _context.CompanyBuilding.Where(b => b.BuildingName == buildingName)
                       from company in companyBD.Company
                       select new
                       {
                           companyBD.BuildingName,
                           company
                       };

            var comByFloor = from otherInfo in _context.CompanyOtherInfo.Where(info => info.Floor.Contains(floor))
                             select new
                             {
                                 otherInfo
                             };

            var data = from cb in comByBuilidng
                        join cf in comByFloor on cb.company.CompanyName equals cf.otherInfo.CompanyName
                        select new
                        {
                            cb.BuildingName,
                            cb.company,
                            cb.company.CompanyEconomy,
                            cf.otherInfo
                        };

            return data;   
        }


        public IQueryable<object> GetFloorsByBuilding(int id)
        {
            var floorsInfo = from bd in _context.CompanyBuilding.Where(cb => cb.Id == id)
                             select new
                             {
                                 bd.Floor
                             };

            return floorsInfo;

        }

        //public IQueryable<object> GetCompanyTaxInfo()
        //{
        //    return _context.CompanyTaxInfo;
        //}
    }
}
