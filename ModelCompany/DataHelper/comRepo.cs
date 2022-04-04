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
            var data = from cb in _context.CompanyBasicInfo.Where(c => c.CompanyName.Contains(serchName) || c.UnifiedSocialCreditCode.Contains(serchName))
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
            try
            {
                var countTax = (from ct in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                                orderby ct.Tax descending
                                select new
                                {
                                    ct.CompanyBasicInfo.CompanyName,
                                    cTax = ct.Tax.ToString("F2")
                                }).Take(10);
                return countTax;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定街道税收前十
        public IQueryable<object> GetTaxTop()
        {
            try
            {
                var countTax = (from ct in _context.CompanyTax.Where(cb => cb.Year == 2020)
                                orderby ct.Tax descending
                                select new
                                {
                                    ct.CompanyBasicInfo.CompanyName,
                                    cTax = ct.Tax.ToString("F2")
                                }).Take(10);
                return countTax;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //public IQueryable<object> Getoutcompoint()
        //{
        //    try
        //    {
        //        var outsideCompanyPoint = from c in _context.OutsideCompany
        //                                  select new
        //                                  {
        //                                      sign = "outsideCompany",
        //                                      c
        //                                  };
        //        return outsideCompanyPoint;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
        public IEnumerable<object> GetTaxTopOnMap()
        {
            try
            {
                var query = (from a in _context.CompanyTax.Where(cb => cb.Year == 2020)
                             join b in _context.CompanyRoom
                             on a.CompanyBasicInfo.Id equals b.CompanyBasicInfo.Id
                             orderby a.Tax descending
                             select new
                             {
                                 name = a.CompanyBasicInfo.CompanyName,
                                 longitude = b.Long,
                                 latitude = b.Lat,
                                 height = b.Height,
                                 tax = a.Tax.ToString("F2"),
                                 enterprise = a.CompanyBasicInfo.EnterpriseType,
                                 industryname = a.CompanyBasicInfo.IndustryName,
                                 contact = a.CompanyBasicInfo.Contacts,
                                 phhone = a.CompanyBasicInfo.Phone
                             }).ToList();
                var list = (from p in query
                            group p by new { p.name }
                           into mygroup
                            select mygroup.FirstOrDefault()).Take(10);
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定楼栋营收前十
        public IQueryable<object> GetCountRevenueByBuilding(string buildingName)
        {
            try
            {
                var countRevenue = (from cr in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                                    orderby cr.Revenue descending
                                    select new
                                    {
                                        cr.CompanyBasicInfo.CompanyName,
                                        cRevenue = cr.Revenue.ToString("F2")
                                    }).Take(10);
                return countRevenue;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回街道营收前十
        public IQueryable<object> GetRevenueTop()
        {
            try
            {
                var countRevenue = (from cr in _context.CompanyTax.Where(cb => cb.Year == 2020)
                                    orderby cr.Revenue descending
                                    select new
                                    {
                                        cr.CompanyBasicInfo.CompanyName,
                                        cRevenue = cr.Revenue.ToString("F2")
                                    }).Take(10);
                return countRevenue;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<object> GetRevenueTopOnMap()
        {
            try
            {
                var query = (from a in _context.CompanyTax.Where(cb => cb.Year == 2020)
                             join b in _context.CompanyRoom
                             on a.CompanyBasicInfo.Id equals b.CompanyBasicInfo.Id
                             orderby a.Revenue descending
                             select new
                             {
                                 name = a.CompanyBasicInfo.CompanyName,
                                 longitude = b.Long,
                                 latitude = b.Lat,
                                 height = b.Height,
                                 revenue = a.Revenue.ToString("F2"),
                                 enterprise = a.CompanyBasicInfo.EnterpriseType,
                                 industryname = a.CompanyBasicInfo.IndustryName,
                                 contact = a.CompanyBasicInfo.Contacts,
                                 phhone = a.CompanyBasicInfo.Phone
                             }).ToList();
                var list = (from p in query
                            group p by new { p.name }
                           into mygroup
                            select mygroup.FirstOrDefault()).Take(10);
                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定楼栋总公司数量、总税收、总营收
        public IQueryable<object> GetTotalTaRByBuilding(string buildingName)
        {
            try
            {
                var totalTax = from tt in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                               group tt by tt.Year into g
                               select new
                               {
                                   g.Key,
                                   companyCount = g.Count(),
                                   tTax = g.Sum(tt => tt.Tax).ToString("F2"),
                                   tRevenue = g.Sum(tt => tt.Revenue).ToString("F2")
                               };
                return totalTax;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定街道总公司数量、总税收、总营收
        public IQueryable<object> GetTotalTaRNO()
        {
            try
            {
                var totalTax = from tt in _context.CompanyTax.Where(cb => cb.Year == 2020)
                               group tt by tt.Year into g
                               select new
                               {
                                   g.Key,
                                   companyCount = g.Count(),
                                   tTax = g.Sum(tt => tt.Tax).ToString("F2"),
                                   tRevenue = g.Sum(tt => tt.Revenue).ToString("F2")
                               };
                return totalTax;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回楼宇总公司数量、总税收、总营收
        public IQueryable<object> GetTotalTaR()
        {
            try
            {
                var totalTax = from tt in _context.CompanyBuildings
                               from ct in tt.CompanyBasicInfo
                               from rt in ct.CompanyTax.Where(zt => zt.Year == 2020)
                               group rt by tt.BuildingName into g
                               orderby g.Sum(tt => tt.Revenue) descending
                               select new
                               {
                                   g.Key,
                                   companyCount = g.Count(),
                                   tTax = g.Sum(tt => tt.Tax).ToString("F2"),
                                   tRevenue = g.Sum(tt => tt.Revenue).ToString("F2")
                               };
                return totalTax;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定楼栋产业分类及产业总营收、税收
        public IQueryable<object> GetIndustryTypeByBuilding(string buildingName)
        {
            try
            {
                string[] industryName = new string[]{ "", "农、林、牧、渔业", "采矿业", "制造业", "电力、燃气及水的生产和供应业", "建筑业", "交通运输仓储和邮政业",
                "信息传输、计算机服务和软件业", "批发和零售业", "住宿和餐饮业", "金融业", "房地产业", "租赁和商务服务业", "科学研究、技术服务和地质勘探业",
                "水利、环境和公共设施管理业", "居民服务和其他服务业", "教育", "卫生、社会保障和社会福利业", "文化体育和娱乐业" };
                var companyCount = from tt in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020 && cb.CompanyBasicInfo.IndustryCode != "")
                                   group tt by tt.CompanyBasicInfo.IndustryCode into g
                                   select new
                                   {
                                       g.Key,
                                       industryName = industryName[int.Parse(g.Key)],
                                       industryCompanyCount = g.Count(),
                                       industryRevenue = g.Sum(tt => tt.Revenue).ToString("F2"),
                                       industryTax = g.Sum(tt => tt.Tax).ToString("F2")
                                   };
                return companyCount;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回街道产业分类及产业总营收、税收
        public IQueryable<object> GetIndustryType()
        {
            try
            {
                string[] industryName = new string[]{ "", "农林牧渔", "采矿业", "制造业", "电力燃气水", "建筑业", "运输仓储邮政业",
                "计算机信息软件业", "批发零售", "住宿餐饮", "金融业", "房地产业", "租赁商务业", "研究技术地质业",
                "水利环境公共管理", "服务业", "教育", "卫生社会保障业", "文娱体育业" };
                var companyCount = from tt in _context.CompanyTax.Where(cb => cb.Year == 2020 && cb.CompanyBasicInfo.IndustryCode != "")
                                   group tt by tt.CompanyBasicInfo.IndustryCode into g
                                   orderby g.Sum(tt => tt.Revenue) descending
                                   select new
                                   {
                                       g.Key,
                                       industryName = industryName[int.Parse(g.Key)],
                                       industryCompanyCount = g.Count(),
                                       industryRevenue = g.Sum(tt => tt.Revenue).ToString("F2"),
                                       industryTax = g.Sum(tt => tt.Tax).ToString("F2")
                                   };
                return companyCount;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定楼栋营收分布
        public IQueryable<object> GetRevenueRoundByBuilding(string buildingName)
        {
            try
            {
                var revenueRound = from ct in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                                   group ct by ct.Year into g
                                   select new
                                   {
                                       r0 = g.Sum(tt => tt.Revenue < 50 ? 1 : 0),
                                       r50 = g.Sum(tt => tt.Revenue >= 50 && tt.Revenue < 100 ? 1 : 0),
                                       r100 = g.Sum(tt => tt.Revenue >= 100 && tt.Revenue < 500 ? 1 : 0),
                                       r500 = g.Sum(tt => tt.Revenue >= 500 && tt.Revenue < 1000 ? 1 : 0),
                                       r1000 = g.Sum(tt => tt.Revenue >= 1000 && tt.Revenue < 5000 ? 1 : 0),
                                       r5000 = g.Sum(tt => tt.Revenue >= 5000 ? 1 : 0)
                                   };
                return revenueRound;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回街道营收分布
        public IQueryable<object> GetRevenueRound()
        {
            try
            {
                var revenueRound = from ct in _context.CompanyTax.Where(cb => cb.Year == 2020)
                                   group ct by ct.Year into g
                                   select new
                                   {
                                       r0 = g.Sum(tt => tt.Revenue < 50 ? 1 : 0),
                                       r50 = g.Sum(tt => tt.Revenue >= 50 && tt.Revenue < 100 ? 1 : 0),
                                       r100 = g.Sum(tt => tt.Revenue >= 100 && tt.Revenue < 500 ? 1 : 0),
                                       r500 = g.Sum(tt => tt.Revenue >= 500 && tt.Revenue < 1000 ? 1 : 0),
                                       r1000 = g.Sum(tt => tt.Revenue >= 1000 && tt.Revenue < 5000 ? 1 : 0),
                                       r5000 = g.Sum(tt => tt.Revenue >= 5000 ? 1 : 0)
                                   };
                return revenueRound;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回指定楼栋税收分布
        public IQueryable<object> GetTaxRoundByBuilding(string buildingName)
        {
            try
            {
                var taxRound = from ct in _context.CompanyTax.Where(cb => cb.CompanyBasicInfo.CompanyBuildings.BuildingName == buildingName && cb.Year == 2020)
                               group ct by ct.Year into g
                               select new
                               {
                                   t0 = g.Sum(tt => tt.Tax < 30 ? 1 : 0),
                                   t30 = g.Sum(tt => tt.Tax >= 30 && tt.Tax < 50 ? 1 : 0),
                                   t50 = g.Sum(tt => tt.Tax >= 50 && tt.Tax < 100 ? 1 : 0),
                                   t100 = g.Sum(tt => tt.Tax >= 100 && tt.Tax < 300 ? 1 : 0),
                                   t300 = g.Sum(tt => tt.Tax >= 300 && tt.Tax < 500 ? 1 : 0),
                                   t500 = g.Sum(tt => tt.Tax >= 500 && tt.Tax < 1000 ? 1 : 0),
                                   t1000 = g.Sum(tt => tt.Tax >= 1000 ? 1 : 0)
                               };
                return taxRound;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //返回街道税收分布
        public IQueryable<object> GetTaxRound()
        {
            try
            {
                var taxRound = from ct in _context.CompanyTax.Where(cb => cb.Year == 2020)
                               group ct by ct.Year into g
                               select new
                               {
                                   t0 = g.Sum(tt => tt.Tax < 30 ? 1 : 0),
                                   t30 = g.Sum(tt => tt.Tax >= 30 && tt.Tax < 50 ? 1 : 0),
                                   t50 = g.Sum(tt => tt.Tax >= 50 && tt.Tax < 100 ? 1 : 0),
                                   t100 = g.Sum(tt => tt.Tax >= 100 && tt.Tax < 300 ? 1 : 0),
                                   t300 = g.Sum(tt => tt.Tax >= 300 && tt.Tax < 500 ? 1 : 0),
                                   t500 = g.Sum(tt => tt.Tax >= 500 && tt.Tax < 1000 ? 1 : 0),
                                   t1000 = g.Sum(tt => tt.Tax >= 1000 ? 1 : 0)
                               };
                return taxRound;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //街道分行业营收前十名
        public IQueryable<object> GetIndustryRevenueTop(string industryCode)
        {
            try
            {
                var industrytop = (from ci in _context.CompanyBasicInfo.Where(r => r.IndustryCode == industryCode)
                                  from ct in ci.CompanyTax.Where(y => y.Year == 2020)
                                  orderby ct.Revenue descending
                                  select new
                                  {
                                      //industrycode = ci.IndustryCode,
                                      companyname = ci.CompanyName,
                                      revenue = ct.Revenue
                                  }).Take(3);
                return industrytop;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        //街道分行业税收前十名
        public IQueryable<object> GetIndustryTaxTop(string industryCode)
        {
            try
            {
                var industrytop = (from ci in _context.CompanyBasicInfo.Where(r => r.IndustryCode == industryCode)
                                   from ct in ci.CompanyTax.Where(y => y.Year == 2020)
                                   orderby ct.Tax descending
                                   select new
                                   {
                                       //industrycode = ci.IndustryCode,
                                       companyname = ci.CompanyName,
                                       tax = ct.Tax
                                   }).Take(3);
                return industrytop;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}