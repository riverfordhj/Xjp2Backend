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

            var comByFloor = from otherInfo in _context.CompanyOtherInfo.Where(info => info.Floor == floor)
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


        //筛选出指定楼栋、楼层的公司信息（中文），用于透视表统计
        public IQueryable<object> GetCompanysByFloor_ZH(string buildingName, string floor)
        {
            var comByBuilidng = from companyBD in _context.CompanyBuilding.Where(b => b.BuildingName == buildingName)
                                from company in companyBD.Company
                                select new
                                {
                                    companyBD.BuildingName,
                                    company
                                };

            var comByFloor = from otherInfo in _context.CompanyOtherInfo.Where(info => info.Floor == floor)
                             select new
                             {
                                 otherInfo
                             };

            //var data = from cb in comByBuilidng
            //           join cf in comByFloor on cb.company.CompanyName equals cf.otherInfo.CompanyName
            //           select new
            //           {
            //               cb.BuildingName,
            //               cb.company,
            //               cb.company.CompanyEconomy,
            //               cf.otherInfo
            //           };
            var data = from cb in comByBuilidng
                       join cf in comByFloor on cb.company.CompanyName equals cf.otherInfo.CompanyName
                       select new
                       {
                           楼宇名称 = cb.BuildingName,
                           企业名称 = cb.company.CompanyName,
                           企业类型 = cb.company.EnterpriseType,
                           企业背景 = cb.company.EnterpriseBackground,
                           注册资本 = cb.company.RegisteredCapital,
                           工商注册地址 = cb.company.RegistrationPlace,
                           税后统计区 = cb.company.TaxStatisticsArea,    
                           楼层 = cf.otherInfo.Floor,
                           租赁或购买 = cf.otherInfo.Category,
                           企业面积 = cf.otherInfo.Area,
                       };

            return data;   
        }

        //筛选出指定楼栋的公司信息（中文），用于透视表统计
        public IQueryable<object> GetCompanysByBuilding_ZH(int id)
        {
            var data = from companyBD in _context.CompanyBuilding.Where(b => b.Id == id)
                       from company in companyBD.Company
                       select new
                       {
                           楼宇名称 = companyBD.BuildingName,
                           企业名称 = company.CompanyName,
                           企业类型 = company.EnterpriseType,
                           企业背景 = company.EnterpriseBackground,
                           注册资本 = company.RegisteredCapital,
                           工商注册地址 = company.RegistrationPlace,
                           税后统计区 = company.TaxStatisticsArea,
                           租赁或购买 = company.CompanyOtherInfo.Category,
                           楼层 = company.CompanyOtherInfo.Floor,
                           企业面积 = company.CompanyOtherInfo.Area
                       };

            return data;

        }
        //返回所有公司信息（中文），用于透视表统计
        public IQueryable<object> GetWholeCompanys_ZH()
        {
            var data = from companyBD in _context.CompanyBuilding
                       from company in companyBD.Company
                       select new
                       {
                           楼宇名称 = companyBD.BuildingName,
                           企业名称 = company.CompanyName,
                           企业类型 = company.EnterpriseType,
                           企业背景 = company.EnterpriseBackground,
                           注册资本 = company.RegisteredCapital,
                           工商注册地址 = company.RegistrationPlace,
                           税后统计区 = company.TaxStatisticsArea,
                           租赁或购买 = company.CompanyOtherInfo.Category,
                           楼层 = company.CompanyOtherInfo.Floor,
                           企业面积 = company.CompanyOtherInfo.Area
                       };


            return data;
        }

        //返回指定楼栋的楼层信息
        public IQueryable<object> GetFloorsByBuilding(int id)
        {
            var floorsInfo = from bd in _context.CompanyBuilding.Where(cb => cb.Id == id)
                             select new
                             {
                                 bd.Floor
                             };

            return floorsInfo;

        }

        //public IQueryable<object> GetInfoByBuildingAndFloor(string buildingName, string floorNum)
        //{
        //    var floorInfo = from bf in _context.BuildingFloor.Where(bf => bf.BuildingName == buildingName && bf.FloorNum == floorNum)
        //                    select new
        //                    {
        //                        bf.Long,
        //                        bf.Lat,
        //                        bf.Height
        //                    };
                            
        //    return floorInfo;

        //}



        //返回中文的公司税收信息，用于透视表统计
        public IQueryable<object> GetCompanyTaxInfo()
        {
            var TaxInfo = from tax in _context.CompanyTaxInfo
                          select new
                          {
                             纳税人 = tax.TaxPayer,
                             纳税年份 = tax.TaxYear,
                             纳税额合计 = tax.TotalTax,
                             营业税 = tax.BusinessTax,
                             增值税 = tax.ValueAddedTax,
                             企所 = tax.CorporateIncomeTax,
                             个所 = tax.IndividualIncomeTax,
                             城建 = tax.UrbanConstructionTax,
                             房产 = tax.RealEstateTax,
                             印花 = tax.StampDuty,
                             土使 = tax.LandUseTax,
                             土增 = tax.LandValueIncrementTax,
                             车船 = tax.VehicleAndVesselTax,
                             契税 = tax.DeedTax,
                             教附 = tax.AdditionalTaxOfEducation,
                             滞纳 = tax.DeedTax,

                          };
            return TaxInfo;
        }

        public IQueryable<object> GetBuildingInfoByStatus(string status)
        {
            var buildings = from cBuilding in _context.CompanyBuilding.Where(cb => cb.Status == status)
                            select new
                            {  
                                cBuilding.Id,
                                cBuilding.BuildingName,
                                cBuilding.StartTime,
                                cBuilding.CompletionTime,
                                cBuilding.Status,
                                cBuilding.LegalEntity,
                                cBuilding.ConstructionSite,
                            };
            return buildings;
        }
    }
}
