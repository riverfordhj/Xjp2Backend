using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelCompany
{
    public class CompanyRoom
    {
        [Key]
        public int Id { get; set; }
        [Required]
        //房间号
        public string Name { get; set; }
        public string FloorNum { get; set; }
        public double Long { get; set; }
        public double Lat { get; set; }
        public double Height { get; set; }
        public string Note { get; set; }

        //导航属性
        public CompanyBasicInfo CompanyBasicInfo { get; set; }
        public CompanyBuildings CompanyBuildings { get; set; }
    }
}
