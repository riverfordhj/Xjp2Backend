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
       public List<Company> Company { get; set; }
    }

}
