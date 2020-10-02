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
                           company_OtherInfo = company.Company_OtherInfo
                       };


            return data;
        }

        public IQueryable<object> GetbuildingEco()
        {
            var data = from companyBD in _context.CompanyBuilding
                       from company in companyBD.Company
                       select new
                       {
                           company,
                           companyBD.BuildingName,
                           companyBD.StreetName,
                           companyEco = company.CompanyEconomy,
                           company_OtherInfo = company.Company_OtherInfo
                       };


            return data;
        }

        public IQueryable<object> getCompanysByBuilding(int id)
        {
            var data = from companyBD in _context.CompanyBuilding.Where(b => b.Id == id)
                       from company in companyBD.Company
                       select new
                       {
                           company,
                           companyBD.BuildingName,
                           companyBD.StreetName,
                           companyEco = company.CompanyEconomy,
                           company_OtherInfo = company.Company_OtherInfo
                       };

            return data;

        }


        public IQueryable<object> getDemoData()
        {
            var demoData = from companyBD in _context.CompanyBuilding
                       from company in companyBD.Company
                       select new
                       {
                           company.Contacts,
                           company.Phone,
                           company.BusinessDirection
                       };

            return demoData;

        }
    }
}
