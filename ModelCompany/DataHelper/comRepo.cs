using System;
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
                                       select new
                                       {
                                           croom.CompanyBasicInfo,
                                           croom.CompanyBuildings.BuildingName,
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
                       select new
                       {
                           buildingName = companyBD.BuildingName,
                           area = co.OfficeArea,
                           floorNum = cb.FloorNum,
                           cb.CompanyRoom,
                           cb
                       };
            return data;

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
        public IQueryable<object> GetBuildingFloor(string buildingName)
        {
            var floorInfo = from croom in _context.CompanyBasicInfo.Where(c => c.CompanyBuildings.BuildingName == buildingName)                        
                            select new
                            {
                                croom.FloorNum,              
                            };

            return floorInfo; //.Select(p =>p.FloorNum).Distinct();

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
                       };

            return data;

        }
        //返回所有公司信息（中文），用于透视表统计
        public IQueryable<object> GetWholeCompanys_ZH()
        {
            var data = from companyBD in _context.CompanyBasicInfo
                       from co in companyBD.CompanyOtherInfo
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
                           企业面积 = co.OfficeArea
                       };


            return data;
        }
        #endregion
    }
}
