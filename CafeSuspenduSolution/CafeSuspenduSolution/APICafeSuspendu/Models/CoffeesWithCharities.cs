using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Models
{
    public class CoffeesWithCharities
    {
        public ApplicationUser Cafe { get; set; }
        public List<Charity> Charities { get; set; }
    }
}