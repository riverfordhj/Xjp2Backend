using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Models
{
    /// <summary>
    /// 街道
    /// </summary>
    public class StreetUnit
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //街道名字
        public string Name { get; set; }
        //别名
        public string Alias { get; set; }
        public string Note { get; set; }
        public List<Community> Communities { get; set; }
        public List<Subdivision> Subdivisions { get; set; }

    }

    /// <summary>
    /// 社区
    /// </summary>
    public class Community
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //社区
        public string Name { get; set; }
        //别名
        public string Alias { get; set; }
        public string Note { get; set; }

        //导航属性
        public List<NetGrid> NetGrids { get; set; }
        public List<Subdivision> Subdivisions { get; set; }
        public StreetUnit Street { get; set; }
    }

    /// <summary>
    /// 网格
    /// </summary>
    public class NetGrid
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //网格
        public string Name { get; set; }
        //别名
        public string Alias { get; set; }
        public string Note { get; set; }
        //导航属性
        public List<Building> Buildings { get; set; }
        public Community Community { get; set; }
        //网格员用户
        public User User { get; set; }
    }
    /// <summary>
    /// 小区
    /// </summary>
    public class Subdivision
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //小区
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Note { get; set; }

        //导航属性
        public List<Building> Buildings { get; set; }
        public Community Community { get; set; }
        public StreetUnit Street { get; set; }
    }

    /// <summary>
    /// 楼栋
    /// </summary>
    public class Building
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //楼栋
        public string Name { get; set; }
        public string Alias { get; set; }
        //地址
        public string Address { get; set; }
        public string Note { get; set; }

        //导航属性
        public List<Room> Rooms { get; set; }
        public NetGrid NetGrid { get; set; }
        //public Community Community { get; set; }
        public Subdivision Subdivision { get; set; }
    }

    /// <summary>
    /// 房间
    /// </summary>
    public class Room
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //房间号
        public string Name { get; set; }
        public string Alias { get; set; }       
        //房屋类别
        public string Category { get; set; }
        //房屋用途
        public string Use { get; set; }
        //面积
        public string Area { get; set; }
        //经度
        public double Longitude { get; set; }
        //纬度
        public double Latitude { get; set; }
        //高程
        public double Height { get; set; }
        //其他
        public string Other { get; set; }


        public string Note { get; set; }

        //导航属性
        public List<PersonRoom> PersonRooms { get; set; }
        public Building Building { get; set; }

        [NotMapped]
        public List<Person> Persons
        {
            get
            {
                if (PersonRooms != null)
                    return PersonRooms.Select(item => item.Person).ToList();
                else
                    return null;
            }
        }
    }
}
