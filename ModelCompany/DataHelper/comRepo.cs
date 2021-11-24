﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace ModelCompany.DataHelper
{
    public class comRepo
    {
        private CompanyContext _context;
        public comRepo()
        {
            _context = new CompanyContext();
        }

        public comRepo(CompanyContext context)
        {
            _context = context;
        }


        public IQueryable<object> GetCompanysByRoom(string buildingName, string roomName)
        {
            try
            {
                var roomsWithCompany = from croom in _context.CompanyRoom.Where(r => r.CompanyBuildings.BuildingName == buildingName && r.Name == roomName)  
                                       from oth in croom.CompanyBasicInfo.CompanyOtherInfo
                                       select new
                                       {
                                           croom.CompanyBasicInfo,
                                           croom.CompanyBuildings.BuildingName,
                                           EmployeesNum = oth.EmployeesNum,
                                           BachelorAboveNum = oth.BachelorAboveNum,
                                           PatentNum = oth.PatentNum,
                                           OfficeArea = oth.OfficeArea,
                                           ctax = croom.CompanyBasicInfo.CompanyTax,
                                       };
         
                return roomsWithCompany;
            }
            catch (Exception e)
            {
                return null;
            }
            
        }

        public IQueryable<object> GetCompanysByBuilding(int id)
        {
            //查询到一栋楼宇所有的公司
            var data = from companyBD in _context.CompanyBuildings.Where(b => b.Id == id)
                       from cb in companyBD.CompanyBasicInfo
                       from co in cb.CompanyOtherInfo
                       //from ctax in cb.CompanyTax
                       select new
                       {
                           buildingName = companyBD.BuildingName,
                           area = co.OfficeArea,
                           floorNum = cb.FloorNum,
                           EmployeesNum = co.EmployeesNum,
                           BachelorAboveNum = co.BachelorAboveNum,
                           PatentNum = co.PatentNum,
                           cb.CompanyRoom,
                           ctax = cb.CompanyTax,
                           cb
                       };
            return data;

        }

         public IQueryable<object> GetRoomByBuilding(string buildingName)
        {
            var roomInfo = from croom in _context.CompanyRoom.Where(cr => cr.CompanyBuildings.BuildingName == buildingName)
                            select new
                            {
                                croom.Name,
                            };

            return roomInfo;

        }

        public IQueryable<object> GetBuildingFloor(string buildingName)
        {
            var floorInfo = from croom in _context.CompanyBasicInfo.Where(c => c.CompanyBuildings.BuildingName == buildingName)
                            select new
                            {
                                croom.FloorNum,
                            };

            return floorInfo;

        }

        public IEnumerable<object> GetCompanyBySearch(string serchName)
        {
            var data = from cb in _context.CompanyBasicInfo.Where(c =>c.CompanyName.Contains(serchName) || c.UnifiedSocialCreditCode.Contains(serchName))                     
                       from co in cb.CompanyOtherInfo
                       select new
                       {
                           buildingName = cb.CompanyBuildings.BuildingName,
                           area = co.OfficeArea,
                           floorNum = cb.FloorNum,
                           cb.CompanyRoom,
                           cb
                       };
            return data;
        }

        public IQueryable<object> GetInfoByBuildingAndFloor(string buildingName, string floorNum)
        {
            var floorInfo = from broom in _context.CompanyRoom.Where(bf => bf.CompanyBuildings.BuildingName == buildingName && bf.Name == floorNum)
                            select new
                            {
                                broom.Long,
                                broom.Lat,
                                broom.Height
                            };

            return floorInfo;

        }
        //返回指定楼栋的楼层信息
        public IQueryable<object> GetFloorsByBuilding(int id)
        {
            var floorsInfo = from bd in _context.CompanyBasicInfo.Where(cb => cb.CompanyBuildings.Id == id)
                             select new
                             {
                                 bd.FloorNum
                             };

            return floorsInfo;

        }

        #region 楼宇统计分析
        //筛选出指定楼栋的公司信息（中文），用于透视表统计
        public IQueryable<object> GetCompanysByBuilding_ZH(int id)
        {
            var data = from companyBD in _context.CompanyBuildings.Where(b => b.Id == id)
                       from company in companyBD.CompanyBasicInfo
                       from co in company.CompanyOtherInfo
                       from tax in company.CompanyTax
                       select new
                       {
                           楼宇名称 = companyBD.BuildingName,
                           企业名称 = company.CompanyName,
                           企业类型 = company.EnterpriseType,
                           注册资本 = company.RegisteredCapital,
                           工商注册地址 = company.RegisteredAddress,
                          // 税后统计区 = company.TaxStatisticsArea,
                           租赁或购买 = company.OfficeSpaceType,
                           楼层 = company.FloorNum,
                           企业面积 = co.OfficeArea,
                           年份 = tax.Year,
                           税收 = tax.Tax,
                           营收 = tax.Revenue,
                           产业类型 = company.IndustryCode,
                       };

            return data;

        }
        //返回所有公司信息（中文），用于透视表统计
        public IQueryable<object> GetWholeCompanys_ZH()
        {
            var data = from companyBD in _context.CompanyBasicInfo
                       from co in companyBD.CompanyOtherInfo
                       from tax in companyBD.CompanyTax
                       select new
                       {
                           楼宇名称 = companyBD.CompanyBuildings.BuildingName,
                           企业名称 = companyBD.CompanyName,
                           企业类型 = companyBD.EnterpriseType,
                           注册资本 = companyBD.RegisteredCapital,
                           工商注册地址 = companyBD.RegisteredAddress,
                           //税后统计区 = companyBD.TaxStatisticsArea,
                           租赁或购买 = companyBD.OfficeSpaceType,
                           楼层 = companyBD.FloorNum,
                           企业面积 = co.OfficeArea,
                           年份 = tax.Year,
                           税收 = tax.Tax,
                           营收 = tax.Revenue,
                           产业类型 = companyBD.IndustryCode,
                       };


            return data;
        }
        #endregion






        public IQueryable<object> GetByBuilding(int id)
        {
            //查询到一栋楼宇所有的公司
            var data = from companyBD in _context.CompanyBuildings.Where(b => b.Id == id)
                       from cb in companyBD.CompanyBasicInfo
                       from co in cb.CompanyOtherInfo
                           //from ctax in cb.CompanyTax
                       select new
                       {
                           buildingName = companyBD.BuildingName,
                           area = co.OfficeArea,
                           floorNum = cb.FloorNum,
                           EmployeesNum = co.EmployeesNum,
                           BachelorAboveNum = co.BachelorAboveNum,
                           PatentNum = co.PatentNum,
                           cb.CompanyRoom,
                           ctax = cb.CompanyTax,
                           cb
                       };
            return data;

        }

        //返回指定楼栋税收前十
        public IQueryable<object> GetCountTaxByBuilding(string buildingName)
        {
            var countTax = (from ct in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                            orderby ct.Tax descending
                            select new
                            {
                                ct.CompanyBasicInfo.CompanyName,
                                ct.Tax
                            }).Take(10);
            return countTax;
        }

        //返回指定楼栋营收前十
        public IQueryable<object> GetCountRevenueByBuilding(string buildingName)
        {
            var countRevenue = (from cr in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                                orderby cr.Revenue descending
                                select new 
                                {
                                    cr.CompanyBasicInfo.CompanyName,
                                    cr.Revenue
                                }).Take(10);
            return countRevenue;
        }

        //返回指定楼栋总税收、总营收
        public IQueryable<object> GetTotalTaRByBuilding(string buildingName)
        {
            var totalTax = from tt in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                           group tt by tt.Year into g
                           select new
                           {
                               g.Key,
                               tTax = g.Sum(tt => tt.Tax),
                               tRevenue = g.Sum(tt => tt.Revenue),
                               comCount = g.Count()
                           };
            return totalTax;
        }

        //返回指定楼栋产业分类及产业总营收、税收
        public IQueryable<object> GetIndustryTypeByBuilding(string buildingName)
        {
            var companyCount = from tt in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                               group tt by tt.CompanyBasicInfo.IndustryCode into g
                               select new
                               {
                                   g.Key,
                                   companyCount = g.Count(),
                                   industryRevenue = g.Sum(tt => tt.Revenue),
                                   industryTax = g.Sum(tt => tt.Tax)
                               };
            return companyCount;
        }
    }
}
