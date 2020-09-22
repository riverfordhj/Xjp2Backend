using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class CompanyEconomy
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string UnifiedSocialCreditCode { get; set; }
        public string CorporateTax { get; set; }
        public string duration { get; set; }
    }
}
