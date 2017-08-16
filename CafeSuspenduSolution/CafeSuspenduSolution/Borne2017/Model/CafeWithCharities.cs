using System.Collections.Generic;

namespace Borne2017.Model
{
    public class CafeWithCharities
    {
        public ApplicationUser Cafe { get; set; }

        public List<Charity> Charities { get; set; }
    }
}
