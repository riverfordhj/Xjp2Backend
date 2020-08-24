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
        public string PersonId { get; set; }
        public string Name { get; set; }
        public string EthnicGroups { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool IsHouseholder { get; set; }
        public string RelationWithHouseholder { get; set; }
        public bool IsOwner { get; set; }
        public bool IsLiveHere { get; set; }
        public string Company { get; set; }
        public string PoliticalState { get; set; }
        public string OrganizationalRelation { get; set; }
        public bool IsOverseasChinese { get; set; }
        public bool IsMerried { get; set; }
        public string PopulationCharacter { get; set; }
        public string LodgingReason { get; set; }
        //备注
        public string Note { get; set; }

        //导航属性
        public Room Room { get; set; }
        //public List<Post> Posts { get; set; }
    }
}
