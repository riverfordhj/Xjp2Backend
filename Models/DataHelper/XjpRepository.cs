﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.DataHelper
{
    /// <summary>
    /// 数据访问层，封装访问EF操作
    /// </summary>
    public class XjpRepository
    {
        private StreetContext _context;
        public XjpRepository()
        {
            _context = new StreetContext();
        }

        public XjpRepository(StreetContext context)
        {
            _context = context;
        }

        #region street
        public void AddStreet(StreetUnit street)
        {
            _context.Streets.Add(street);
        }

        public void DeleteStreet(StreetUnit street)
        {
            _context.Streets.Remove(street);
        }

        public StreetUnit GetStreet(int id)
        {
            return _context.Streets.Where(item => item.Id == id).FirstOrDefault();
        }
        public StreetUnit GetStreet(string name)
        {
            return _context.Streets.Where(item => item.Name == name).FirstOrDefault();
        }

        public IEnumerable<StreetUnit> GetStreets()
        {
            return _context.Streets.ToList();//.Take(10)
        }

        public bool StreetExists(int id)
        {
            return _context.Streets.Any(item => item.Id == id);
        }
        #endregion

        #region 社区
        public Community GetCommunity(string name)
        {
            return _context.Communitys.Where(item => item.Name == name).FirstOrDefault();
        }
        #endregion

        #region 小区
        public Subdivision GetSubdivision(string name)
        {
            return _context.Subdivisions.Where(item => item.Name == name || item.Alias.Contains(name)).FirstOrDefault();
        }
        #endregion

        #region building
        public Building GetBuildingInCommunity(string commnunityName, string name)
        {
            //Community com = GetCommunity(commnunityName);
            //if (com != null)
            //    return _context.Buildings.Where(item => item.Community == com && item.Name == name).FirstOrDefault();
            //else
            return null;
        }

        public Building GetBuildingInSubdivision(string subdivisionName, string name)
        {
            Subdivision sub = GetSubdivision(subdivisionName);
            if (sub != null)
            {
                return _context.Buildings.Where(item => item.Subdivision.Id == sub.Id && (item.Name == name || item.Alias == name)).FirstOrDefault();
            }
            else
                return null;
        }

        /// <summary>
        /// 获取小区内的楼栋
        /// </summary>
        /// <param name="id">小区 id</param>
        /// <returns></returns>
        public IQueryable<Building> GetBuildingInSubdivision(int id)
        {
            return _context.Buildings.Where(item => item.Subdivision != null && item.Subdivision.Id == id);
        }
        #endregion

        #region Room
        public Room GetRoom(string subdivisionName, string buidlingName, string roomNO)
        {
            Building building = GetBuildingInSubdivision(subdivisionName, buidlingName);
            if (building != null)
                return _context.Rooms.SingleOrDefault(item => item.Building.Id == building.Id && item.Name == roomNO);
            else
                return null;
        }
        public Room GetRoom(int buidlingId, string roomNO)
        {
            return _context.Rooms.SingleOrDefault(item => item.Building.Id == buidlingId && item.Name == roomNO);
        }
        #endregion

        #region 人员
        /// <summary>
        /// 获取建筑物的住户信息
        /// </summary>
        /// <param name="id">建筑物 id </param>
        /// <returns></returns>
        public IEnumerable<object> GetPersonsByBuilding(int id)
        {
            //HashSet<int> personIDs = _context.PersonRooms.Where(item => item.Room.Building.Id == id).Select(item => item.Person.Id).ToHashSet();
            //return _context.Persons.Where(item => personIDs.Contains(item.Id));

            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms.Where(r => r.Building.Id == id)
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };

                //组连接，附加特殊人群信息
                //var ps = from p in _context.Persons
                //         where p.PersonId == "35220119860918511X"
                //         select new
                //        {
                //            p.PersonId,
                //            p.Name
                //        };
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.PersonId,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               SpecialGroup = psg // 特殊人群信息
                           };

                //var d = data.ToList();// ToLookup(sp => sp.p.PersonId, sp => sp.SpecialGroup);
                //var d1 = data as IEnumerable<object>;
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        //获取建筑物的住户信息(中文)
        public IEnumerable<object> GetPersonsByBuilding_ZH(int id)
        {            
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms.Where(r => r.Building.Id == id)
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };

                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               房间号 = pr.RoomNO,
                               社区名 = pr.CommunityName,
                               小区名 = pr.SubdivsionName,
                               楼栋名 = pr.BulidingName,
                               产权人 = pr.IsOwner,
                               户主 = pr.IsHouseholder,
                               在此居住 = pr.IsLiveHere,
                               与户主关系 = pr.RelationWithHouseholder,
                               寄住原因 = pr.LodgingReason,
                               人口性质 = pr.PopulationCharacter,
                           };

                return data;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        ///<summary>
        ///
        /// 根据小区获得人员信息表
        /// 
        ///</summary>
        public IEnumerable<object> GetPersonsBySubdivision(int id)
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms.Where(r => r.Building.Subdivision.Id == id)
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = pr.Room.Building.Name,
                                           SubdivsionName = pr.Room.Building.Subdivision.Name,
                                           CommunityName = pr.Room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };

                //组连接，附加特殊人群信息
                //var ps = from p in _context.Persons
                //         where p.PersonId == "35220119860918511X"
                //         select new
                //        {
                //            p.PersonId,
                //            p.Name
                //        };
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.PersonId,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               SpecialGroup = psg // 特殊人群信息
                           };

                //var d = data.ToList();// ToLookup(sp => sp.p.PersonId, sp => sp.SpecialGroup);
                //var d1 = data as IEnumerable<object>;
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        ///<summery>
        ///通过姓名身份证号电话搜索
        /// </summery>
        public IEnumerable<object> GetPersonsBySearch(string SubdivisionId, string Name)
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from person in _context.Persons.Where(r => r.Name.Contains(Name) || r.PersonId == Name || r.Phone == Name)
                                       from pr in person.PersonRooms
                                       select new
                                       {
                                           RoomId = pr.Room.Id,
                                           RoomNO = pr.Room.Name,
                                           BulidingName = pr.Room.Building.Name,
                                           SubdivsionName = pr.Room.Building.Subdivision.Name,
                                           SubdivisionId = pr.Room.Building.Subdivision.Id.ToString(),
                                           CommunityName = pr.Room.Building.Subdivision.Community.Name,
                                           // person.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };
                if (SubdivisionId != null && SubdivisionId != string.Empty)
                {
                    roomsWithPersons = roomsWithPersons.Where(item => item.SubdivisionId == SubdivisionId);       //.Contains(SubdivisionId));  //== SubdivisionId );

                }
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.Person.PersonId equals sg.PersonId into psg // 根据身份证关联
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.Person.PersonId,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               SpecialGroup = psg // 特殊人群信息
                           };
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        /// <summary>
        /// 获取特殊人群
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetSpecialGroups()
        {
            try
            {
                //根据 room - person 数据
                var roomsWithPersons = from room in _context.Rooms
                                       from pr in room.PersonRooms
                                       select new
                                       {
                                           RoomId = room.Id,
                                           RoomNO = room.Name,
                                           BulidingName = room.Building.Name,
                                           SubdivsionName = room.Building.Subdivision.Name,
                                           CommunityName = room.Building.Subdivision.Community.Name,
                                           pr.PersonId,
                                           pr.Person,
                                           IsOwner = pr.IsOwner ? "是" : "否",
                                           IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                           IsLiveHere = pr.IsLiveHere ? "是" : "否",
                                           pr.RelationWithHouseholder,
                                           pr.LodgingReason,
                                           pr.PopulationCharacter
                                       };
                var psdata = roomsWithPersons.ToList();

                var data = from pr in psdata
                           join sg in _context.SpecialGroups on pr.PersonId equals sg.PersonId //into psg // 根据身份证关联
                           //where pr.PersonId = sg.PersonId
                           select new
                           {
                               pr.RoomId,
                               pr.RoomNO,
                               pr.CommunityName,
                               pr.SubdivsionName,
                               pr.BulidingName,
                               pr.PersonId,
                               pr.Person,
                               pr.IsOwner,
                               pr.IsHouseholder,
                               pr.IsLiveHere,
                               pr.RelationWithHouseholder,
                               pr.LodgingReason,
                               pr.PopulationCharacter,
                               sg.Type,
                               //SpecialGroup = psg // 特殊人群信息

                           };
                return data;
            }
            catch (Exception e)
            {
                return null;
            }


        }
        /// <summary>
        /// 根据房间号，获取住房人员信息列表
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public IQueryable<Person> GetPersonsInRoom(int roomId)
        {
            List<int> personIDs = _context.PersonRooms.Where(item => item.Room.Id == roomId).Select(item => item.Person.Id).ToList();
            return _context.Persons.Where(item => personIDs.Contains(item.Id));
        }

        /// <summary>
        /// 通过社区名、楼栋名、房号，获取住房人信息
        /// </summary>
        /// <param name="commnunityName"></param>
        /// <param name="buidlingName"></param>
        /// <param name="roomNO"></param>
        /// <returns></returns>
        public IQueryable<Person> GetPersonsInRoom(string subdivisionName, string buidlingName, string roomNO)
        {
            Room room = GetRoom(subdivisionName, buidlingName, roomNO);

            if (room != null)
            {
                List<int> personIDs = _context.PersonRooms.Where(item => item.Room == room).Select(item => item.Person.Id).ToList();
                return _context.Persons.Where(item => personIDs.Contains(item.Id));
            }
            else
                return null;
        }
        #endregion


        #region 用户、角色
        
        public User GetUserByName(string name)
        {
            var user = _context.Users.Include(u => u.RoleUsers).ThenInclude(ru => ru.Role).SingleOrDefault(r => r.UserName.Equals(name));
            //加载role信息
            //_context.Entry(user).Collection(u => u.RoleUsers).Load();
            return user;
        }
        
        //根据user，返回对应的小区
        public IQueryable<object> GetSubdivsionsByUser(string userName)
        {  

            //var isNetGrid = roleList.Find(role => role.Name == "网格员");

            var user = GetUserByName(userName);
            var roleList = user.Roles;

            if(roleList[0].Name == "网格员")
            {
                return (from useRow in _context.Users.Where(u => u.UserName == userName)
                        from grid in useRow.NetGrid
                        select new
                        {
                            grid.Community.Subdivisions[0].Id,
                            grid.Community.Subdivisions[0].Name
                        });
            
            }

            if (roleList[0].Name == "水岸星城")
            {
                return (from comm in _context.Communitys.Where(u => u.Name == "水岸星城")
                        select new
                        {
                            comm.Subdivisions[0].Id,
                            comm.Subdivisions[0].Name

                        });
            }

            //if (roleList[0].Name == "Administrator" && roleList[1].Name == "网格员")
            //{
            //    return _context.Subdivisions;
            //}

            return _context.Subdivisions;
        }

        //根据网格员，返回相应楼栋
        public IQueryable<object> GetBuildingsByNetGrid(string userName)
        {
            return from u in _context.Users.Where(u => u.UserName == userName)
                   from ng in u.NetGrid
                   from b in ng.Buildings
                   select new
                   {
                       b.Id,
                       buildingName = b.Name
                   };
        }
        //根据楼栋，返回房屋
        public IQueryable<object> GetRoomsByBuilding(string userName, string buildingName)
        {
            return from u in _context.Users.Where(u => u.UserName == userName)
                   from ng in u.NetGrid
                   from b in ng.Buildings.Where(b => b.Name == buildingName)
                   from r in b.Rooms
                   select new
                   {
                       r.Id,
                       roomName = r.Name,
                       netGrid = ng.Name,
                       communityName = ng.Community.Name
                   };
        }

        public IEnumerable<object> GetPersonsByUser(string userName){
            var user = GetUserByName(userName);
            var roleList = user.Roles;

            //网格
            if (roleList[0].Name == "网格员")
            {
                var personHouseInfo = from u in _context.Users.Where(u => u.UserName == userName)
                                 from ng in u.NetGrid
                                 from b in ng.Buildings
                                 from room in b.Rooms
                                 from pr in room.PersonRooms
                                 select new
                                 {
                                    pr.Person.PersonId,
                                    pr.Person.Name,
                                    pr.Person.Phone,
                                    pr.Status,
                                    RoomName = room.Name,
                                    RoomUse = room.Use,
                                    room.Category,
                                    BuildingName = b.Name,
                                    NetGrid = ng.Name,
                                    CommunityName = ng.Community.Name,
                                    pr.PopulationCharacter,
                                    IsHouseholder = pr.IsHouseholder ? "是" : "否",
                                    pr.RelationWithHouseholder
                                 };

                var personHouseEditInfo = from phei in _context.PersonHouseDatas.Where(phd => phd.Status != "rejected" && phd.Status != "approved" && phd.Operation == "creating" && phd.Editor == userName )
                                          select new
                                          {
                                              phei.PersonId,
                                              phei.Name,
                                              phei.Phone,
                                              phei.Status,
                                              phei.RoomName,
                                              phei.RoomUse,
                                              phei.Category,
                                              phei.BuildingName,
                                              phei.NetGrid,
                                              phei.CommunityName,
                                              phei.PopulationCharacter,
                                              IsHouseholder = phei.IsHouseholder ? "是" : "否",
                                              phei.RelationWithHouseholder
                                          };

                     return personHouseInfo.AsEnumerable().Union(personHouseEditInfo.AsEnumerable());

            }

            //社区
            if (roleList[0].Name == "水岸星城")
            {                                       
                return SearchPersonHouseInfo("水岸星城", null);

            }

            //街道
            return SearchPersonHouseInfo( null, null);
        }

        //返回指定网格员编辑的数据
        public IQueryable<object> SearchPersonHouseByNetGrid(string editor)
        {
            return from phei in _context.PersonHouseDatas.Where(phd => phd.Status != "rejected" && phd.Status != "approved" && phd.Editor == editor)
                   select new
                   {
                       phei.PersonId,
                       phei.Name,
                       phei.Phone,
                       phei.RoomName,
                       phei.RoomUse,
                       phei.Category,
                       phei.BuildingName,
                       phei.NetGrid,
                       phei.CommunityName,
                       phei.PopulationCharacter,
                       IsHouseholder = phei.IsHouseholder ? "是" : "否",
                       phei.RelationWithHouseholder,
                       phei.Editor,
                       phei.EditTime,
                       phei.Operation,
                       phei.Status
                   };
        }
        //根据用户，返回personHouse数据
        public IQueryable<object> SearchPersonHouseInfo (string communityName, string netGridName)
        {
            if(netGridName != null)
            {
                return SearchPersonHouseByNetGrid(netGridName);
            }
            if (communityName != null)
            {
                return from phei in _context.PersonHouseDatas.Where(phd => phd.Status != "approved" && phd.CommunityName == communityName)
                        select new
                        {
                            phei.PersonId,
                            phei.Name,
                            phei.Phone,
                            phei.Status,
                            phei.RoomName,
                            phei.RoomUse,
                            phei.Category,
                            phei.BuildingName,
                            phei.NetGrid,
                            phei.CommunityName,
                            phei.PopulationCharacter,
                            IsHouseholder = phei.IsHouseholder ? "是" : "否",
                            phei.RelationWithHouseholder
                        };
            }
            //若未指定社区或网格，则返回街道数据
            return from phei in _context.PersonHouseDatas.Where(phd => phd.Status == "approved" || phd.Status == "rejected" || phd.Status == "verified")
                    select new
                    {
                        phei.PersonId,
                        phei.Name,
                        phei.Phone,
                        phei.Status,
                        phei.RoomName,
                        phei.RoomUse,
                        phei.Category,
                        phei.BuildingName,
                        phei.NetGrid,
                        phei.CommunityName,
                        phei.PopulationCharacter,
                        IsHouseholder = phei.IsHouseholder ? "是" : "否",
                        phei.RelationWithHouseholder
                    };
        }

        //网格员修改指定人员信息
        public IEnumerable<object> UpdatePersonHouseByNetGrid(string userName, PersonUpdateParamTesting personFields)
        {          
            PersonHouseData targetPersonHouse = _context.PersonHouseDatas.SingleOrDefault(phd => phd.PersonId == personFields.PersonId
                                                                && phd.RoomName == personFields.RoomName
                                                                && phd.BuildingName == personFields.BuildingName
                                                                && phd.NetGrid == personFields.NetGrid
                                                                && phd.CommunityName == personFields.CommunityName);

            //修改或删除操作是对原有数据进行操作的，设定在对应的personRoom信息条中标明操作状态
            if (personFields.Operation != "creating")
            {
                if(targetPersonHouse == null)
                {
                    var personRoom = _context.PersonRooms.SingleOrDefault(pr => pr.PersonId == personFields.PersonId
                                                                            && pr.Room.Name == personFields.RoomName
                                                                            && pr.Room.Building.Name == personFields.BuildingName
                                                                            && pr.Room.Building.NetGrid.Name == personFields.NetGrid
                                                                            && pr.Room.Building.NetGrid.Community.Name == personFields.CommunityName);
                    personRoom.Status = personFields.Status;

                }
                else if(targetPersonHouse != null)
                {
                    targetPersonHouse.PersonId = personFields.PersonId;
                    targetPersonHouse.Name = personFields.Name;
                    targetPersonHouse.Phone = personFields.Phone;
                    targetPersonHouse.Status = personFields.Status;
                    targetPersonHouse.IsHouseholder = personFields.IsHouseholder == "是" ? true : false;
                    targetPersonHouse.PopulationCharacter = personFields.PopulationCharacter;
                    targetPersonHouse.RoomName = personFields.RoomName;
                    targetPersonHouse.RoomUse = personFields.RoomUse;
                    targetPersonHouse.Category = personFields.Category;
                    targetPersonHouse.BuildingName = personFields.BuildingName;
                    targetPersonHouse.NetGrid = personFields.NetGrid;
                    targetPersonHouse.CommunityName = personFields.CommunityName;
                    targetPersonHouse.Editor = userName;
                    targetPersonHouse.EditTime = DateTime.Now.ToString();
                }
            }

            if (personFields.Operation == "creating" && targetPersonHouse == null)
            {
                targetPersonHouse = new PersonHouseData
                {
                    PersonId = personFields.PersonId,
                    Name = personFields.Name,
                    Phone = personFields.Phone,
                    Status = personFields.Status,
                    IsHouseholder = personFields.IsHouseholder == "是" ? true : false,
                    RelationWithHouseholder = personFields.RelationWithHouseholder,
                    PopulationCharacter = personFields.PopulationCharacter,
                    RoomName = personFields.RoomName,
                    RoomUse = personFields.RoomUse,
                    Category = personFields.Category,
                    BuildingName = personFields.BuildingName,
                    NetGrid = personFields.NetGrid,
                    CommunityName = personFields.CommunityName,
                    Editor = userName,
                    EditTime = DateTime.Now.ToString(),
                    Operation = personFields.Operation
                };
                _context.PersonHouseDatas.Add(targetPersonHouse);
            }
          
           
            _context.SaveChanges();

            return GetPersonsByUser(userName);
                        
        }

        //选中一条personHouseData数据
        public PersonHouseData PickPersonHouse(VerifyAndConfirmParam personHouseFileds)
        {
            return _context.PersonHouseDatas.SingleOrDefault(phd => phd.PersonId == personHouseFileds.PersonId
                                                                && phd.RoomName == personHouseFileds.RoomName
                                                                && phd.BuildingName == personHouseFileds.BuildingName
                                                                && phd.NetGrid == personHouseFileds.NetGrid
                                                                && phd.CommunityName == personHouseFileds.CommunityName);
        }
        //选中一条personRoom数据
        public PersonRoom PickPersonRoom(VerifyAndConfirmParam personHouseFileds)
        {
           return _context.PersonRooms.SingleOrDefault(pr => pr.PersonId == personHouseFileds.PersonId
                                                            && pr.Room.Name == personHouseFileds.RoomName
                                                            && pr.Room.Building.Name == personHouseFileds.BuildingName
                                                            && pr.Room.Building.NetGrid.Name == personHouseFileds.NetGrid
                                                            && pr.Room.Building.NetGrid.Community.Name == personHouseFileds.CommunityName);
        }
        //选中一个房间
        public Room PickRoom(VerifyAndConfirmParam  personHouseFileds) { 
            return _context.Rooms.SingleOrDefault(r => r.Name == personHouseFileds.RoomName
                                                    && r.Building.Name == personHouseFileds.BuildingName
                                                    && r.Building.NetGrid.Name == personHouseFileds.NetGrid
                                                    && r.Building.NetGrid.Community.Name == personHouseFileds.CommunityName);
        }
        //社区审核网格员的提交
        public void VerifyByCommunity(VerifyAndConfirmParam verifyFileds)
        {
            PersonHouseData personHouse = PickPersonHouse(verifyFileds);
            PersonRoom personRoom = PickPersonRoom(verifyFileds);
            personHouse.Status = verifyFileds.Status;
            if(personRoom != null)
            {
               personRoom.Status = verifyFileds.Status;
            }

            _context.SaveChanges();
        }

        //街道审批社区的审核
        public IEnumerable<object> ConfirmByAdmin(VerifyAndConfirmParam confirmFields, string userName)
        {
            PersonHouseData personHouse = PickPersonHouse(confirmFields);
            PersonRoom personRoom = PickPersonRoom(confirmFields);
            personHouse.Status = confirmFields.Status;//personHouseData的状态总会改变

            if (confirmFields.Status == "approved")//街道批准时，执行
            {
                Person targetPerson = _context.Persons.SingleOrDefault(per => per.PersonId == confirmFields.PersonId);
                Room targetRoom = PickRoom(confirmFields);
                //新建一条personRoom
                if (personHouse.Operation == "creating" && personRoom == null && targetRoom != null)
                {
                    targetRoom.Use = personHouse.RoomUse;//更改房屋用途（会影响到关联此room的所有personRoom信息）
                    if(targetPerson == null)
                    {
                        targetPerson = new Person
                        {
                            PersonId = personHouse.PersonId,
                            Name = personHouse.Name,
                            Phone = personHouse.Phone
                        };
                    }
                  
                    _context.Persons.Add(targetPerson);

                    PersonRoom newPersonRoom = new PersonRoom
                    {
                        PersonId = personHouse.PersonId,
                        IsHouseholder = personHouse.IsHouseholder,
                        RelationWithHouseholder = personHouse.RelationWithHouseholder,
                        PopulationCharacter = personHouse.PopulationCharacter,
                    };
                    newPersonRoom.Person = targetPerson;//targetPerson为null时，关联新增的person信息；不为null时，关联该targetPerson
                    newPersonRoom.Room = targetRoom;
                    _context.PersonRooms.Add(newPersonRoom);

                }else if(personHouse.Operation == "updating" && targetPerson != null && targetRoom != null)
                {
                    targetPerson.Name = personHouse.Name;
                    targetPerson.Phone = personHouse.Phone;
                    personRoom.IsHouseholder = personHouse.IsHouseholder;
                    personRoom.RelationWithHouseholder = personHouse.RelationWithHouseholder;
                    personRoom.PopulationCharacter = personHouse.PopulationCharacter;
                    personRoom.Status = null;
                    targetRoom.Use = personHouse.RoomUse;

                }
                else if(personHouse.Operation == "deleting")
                {
                    _context.PersonRooms.Remove(personRoom);
                    var pr_count = _context.PersonRooms.Where(pr => pr.PersonId == confirmFields.PersonId).ToArray().Length;
                    if(pr_count == 0)
                    {
                        _context.Persons.Remove(targetPerson);
                    }
                   
                }
            }
            else if(confirmFields.Status == "rejected" && personRoom != null)//街道不批准时，执行
            {
                personRoom.Status = confirmFields.Status;
            }


            _context.SaveChanges();

            return GetPersonsByUser(userName);
        }

        public IQueryable<object> PersonHouseHistoryInfo()
        {
            return _context.PersonHouseDatas.Where(prd => prd.Status != "approved");
        }

        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        
    }
}
