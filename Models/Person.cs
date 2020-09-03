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
        public string Address { get; set; }

        public string Company { get; set; }
        public string PoliticalState { get; set; }
        public string OrganizationalRelation { get; set; }
        public bool IsOverseasChinese { get; set; }
        public string MerriedStatus { get; set; }

        //备注
        public string Note { get; set; }

        //导航属性
        public List<PersonRoom> PersonRooms { get; set; }
    }
}
