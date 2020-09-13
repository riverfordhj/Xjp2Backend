using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Name { get; set; }
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
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Note { get; set; }

        //导航属性
        public List<NetGrid> NetGrids { get; set; }
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
        public string Name { get; set; }
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
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Note { get; set; }

        //导航属性
        public List<Building> Buildings { get; set; }
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
        public string Name { get; set; }
        public string Alias { get; set; }
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
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Use { get; set; }
        public string Area { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public string Other { get; set; }


        public string Note { get; set; }

        //导航属性
        public List<PersonRoom> PersonRooms { get; set; }
        public Building Building { get; set; }
    }
}
