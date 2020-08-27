using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        // 手机号
        public string phone { get; set; }
        //头像
        public string PicPath { get; set; }

        //导航属性        
        //网格
        public List<NetGrid> NetGrid { get; set; }

        public List<RoleUser> RoleUsers { get; set; }

        [NotMapped]
        public List<Role> Roles
        {
            get
            {
                if (RoleUsers != null)
                    return RoleUsers.Select(item => item.Role).ToList();
                else
                    return null;
            }
        }
    }


}
