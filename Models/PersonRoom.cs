using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class PersonRoom
    {
        [Key]
        public int Id { get; set; }
        //身份证
        public string PersonId { get; set; }

        //户主
        public bool IsHouseholder { get; set; }
        //与户主关系
        public string RelationWithHouseholder { get; set; }
        //是否为产权人
        public bool IsOwner { get; set; }
        public bool IsLiveHere { get; set; }
                
        //人口性质
        public string PopulationCharacter { get; set; }
        //寄住原因
        public string LodgingReason { get; set; }

        //关联属性
        public Person Person { get; set; }
        public Room Room { get; set; }

    }
}
