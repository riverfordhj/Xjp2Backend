using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class PersonHouseData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        //身份证
        public string PersonId { get; set; }

        [Required]
        //姓名   
        public string Name { get; set; }
        //民族
        public string EthnicGroups { get; set; }
        //电话
        public string Phone { get; set; }
        //户籍地址
        public string DomicileAddress { get; set; }
        //从业单位
        public string Company { get; set; }
        //政治面貌
        public string PoliticalState { get; set; }
        //组织关系
        public string OrganizationalRelation { get; set; }
        //是否侨胞
        public bool IsOverseasChinese { get; set; }
        //婚姻状况
        public string MerriedStatus { get; set; }
        //人员备注
        public string Note { get; set; }

        //是否户主
        public bool IsHouseholder { get; set; }
        //与户主关系
        public string RelationWithHouseholder { get; set; }
        //是否为产权人
        public bool IsOwner { get; set; }
        //是否居住在此
        public bool IsLiveHere { get; set; }

        //人口性质
        public string PopulationCharacter { get; set; }
        //寄住原因
        public string LodgingReason { get; set; }

        [Required]
        //房间号
        public string RoomName { get; set; }
        //房间别名
        public string Alias { get; set; }
        //房屋类别
        public string Category { get; set; }
        //房屋用途
        public string RoomUse { get; set; }
        //房屋面积
        public string Area { get; set; }
        //经度
        public double Longitude { get; set; }
        //纬度
        public double Latitude { get; set; }
        //高程
        public double Height { get; set; }
        //其他（房屋）
        public string Other { get; set; }

        //房屋备注
        public string RoomNote { get; set; }
        //编辑时间
        public string EditTime { get; set; }
        //编辑人
        public string Editor { get; set; }
        //编辑类型
        public string Operation { get; set; }
        //状态
        public string Status { get; set; }
        //楼栋名称
        public string BuildingName { get; set; }
        //地址
        public string Address { get; set; }
        //小区名
        public string SubdivisionName { get; set; }
        //网格号
        public string NetGrid { get; set; }
        //社区名
        public string CommunityName { get; set; }

        public string StreetName { get; set; }
    }
}
