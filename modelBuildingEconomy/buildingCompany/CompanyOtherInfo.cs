using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class CompanyOtherInfo
    {
        [Key]
        public int Id { get; set; }

        public string CompanyName { get; set; }
        public string UnifiedSocialCreditCode { get; set; }
        public string Floor { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string SettlingTime { get; set; }
        public string MoveAwayTime { get; set; }
        
    }
}
