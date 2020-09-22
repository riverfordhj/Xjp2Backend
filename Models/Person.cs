using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        //身份证
        [Required]
        public string PersonId { get; set; }
        [Required]
        public string Name { get; set; }
        public string EthnicGroups { get; set; }
        public string Phone { get; set; }
        public string DomicileAddress { get; set; }

        public string Company { get; set; }
        public string PoliticalState { get; set; }
        public string OrganizationalRelation { get; set; }
        public bool IsOverseasChinese { get; set; }
        public string MerriedStatus { get; set; }

        //备注
        public string Note { get; set; }

        //导航属性
        public List<PersonRoom> PersonRooms { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
    }

    //单位信息
    public class CompanyInfo
    {
        [Key]
        public int Id { get; set; }
        //单位名称
        public string Name { get; set; }
        //单位性质
        public string Character { get; set; }
        //统一社会信用代码
        public string SocialId { get; set; }
        //联系人姓名
        public string ContactPerson { get; set; }
        //身份证号
        public string PersonId { get; set; }
        //联系电话
        public string Phone { get; set; }
        //单位面积
        public string Area { get; set; }
        //备注
        public string Note { get; set; }
    }
}
