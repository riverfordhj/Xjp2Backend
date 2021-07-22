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
                       //from room in cb.CompanyRoom
                       select new
                       {
                           buildingName = companyBD.BuildingName,
                           area = co.OfficeArea,
                           ///roomName = cb.CompanyRoom.Name,
                           cb
                       };
            //var ss = from cc in _context.CompanyBasicInfo.Where(b =>b.CompanyBuildings.Id == id)
            //         from bb in cc.CompanyRoom.First()
            //         select new
            //         {

            //         };
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
    }
}
