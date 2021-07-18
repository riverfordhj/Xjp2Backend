using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelCompany
{
    public class CompanyBuildings
    {
       [Key]
       public int Id { get; set; }
       public string BuildingName { get; set; }
       public string StreetName { get; set; }
       public string note { get; set; }

      public List<CompanyBasicInfo> CompanyBasicInfo { get; set; }
      public List<CompanyRoom> CompanyRoom { get; set; }

    }

}
