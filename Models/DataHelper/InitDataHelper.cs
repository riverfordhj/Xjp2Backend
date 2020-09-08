using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Models.DataHelper
{
    public static class InitDataHelper
    {
        /// <summary>
        /// 添加初始数据
        /// </summary>
        public static string AddData(string comName, string gridUser, int gridCount)
        {
            try
            {
                using (var context = new StreetContext())
                {
                    //添加街道
                    var street = AddStreet(context, "徐家棚");

                    //add 网格员 Role
                    var roleWangGe = AddRole(context, "网格员");

                    #region 添加水岸星城社区、网格、网格员账户
                    //添加社区
                    var saxcCommunity = AddCommunity(context, street, comName);

                    //网格
                    AddGrid(context, saxcCommunity, gridCount, roleWangGe, gridUser);
                    #endregion

                    //添加管理员role，user
                    Role adminRole = AddRole(context, "Administrator");
                    User adminUser = AddUser(context, "admin");

                    AddUser2Role(context, adminRole, adminUser);
                    AddUser2Role(context, roleWangGe, adminUser);

                    context.SaveChanges();
                }
                return "Add data OK!";
            }
            catch (Exception err)
            {
                return err.Message;
            }

        }
        //添加街道数据
        private static StreetUnit AddStreet(StreetContext context, string name)
        {
            StreetUnit street = context.Streets.SingleOrDefault(s => s.Name == name);

            if (street == null)
            {
                street = new StreetUnit { Name = name };
                context.Streets.Add(street);
            }

            return street;
        }



        /// <summary>
        /// 添加网格，及网格员
        /// </summary>
        /// <param name="context"></param>
        /// <param name="community">网格所属社区</param>
        /// <param name="role">用户组</param>
        /// <param name="userName">用户名</param>
        private static void AddGrid(StreetContext context, Community community, int gridCount, Role role, string userName)
        {
            for (int i = 1; i <= gridCount; i++)
            {
                //网格
                var netGrid = context.NetGrids.SingleOrDefault(s => s.Name == i.ToString());
                if (netGrid == null)
                {
                    netGrid = new NetGrid { Name = i.ToString() };
                    netGrid.Community = community;
                    context.NetGrids.Add(netGrid);
                }

                //网格员
                var user = AddUser(context, userName + i.ToString());
                AddUser2Role(context, role, user);
                netGrid.User = user;
            }

        }

        private static User AddUser(StreetContext context, string name)
        {
            var user = context.Users.SingleOrDefault(s => s.UserName == name);
            if (user == null)
            {
                user = new User { UserName = name, Password = "123456" };
                context.Users.Add(user);
            }
            return user;
        }

        private static void AddUser2Role(StreetContext context, Role role, User user)
        {
            RoleUser ru = new RoleUser { Role = role, User = user };
            context.RoleUsers.Add(ru);            
        }

        private static Role AddRole(StreetContext context, string name)
        {
            var role = context.Roles.SingleOrDefault(s => s.Name == name);
            if (role == null)
            {
                role = new Role { Name = name };
                context.Roles.Add(role);
            }
            return role;
        }

        private static Community AddCommunity(StreetContext context, StreetUnit street, string name)
        {
            var community = context.Communitys.SingleOrDefault(s => s.Name == name);
            if (community == null)
            {
                community = new Community { Name = name };
                community.Street = street;
                //street.Communities.Add(community);
                context.Communitys.Add(community);
            }
            return community;
        }
    }
}
