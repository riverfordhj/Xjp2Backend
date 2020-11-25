using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelsBuildingEconomy.buildingCompany
{
    public class CompanyBuilding
    {
       [Key]
       public int Id { get; set; }
       public string BuildingName { get; set; }
       public string StreetName { get; set; }
       public string LegalEntity { get; set; }
       public string ConstructionSite { get; set; }
       public string StartTime { get; set; }
       public string CompletionTime { get; set; }
       public string Status { get; set; }
       public List<BuildingFloor> Floor { get; set; }
       public List<Company> Company { get; set; }
    }

}
