using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Model
{
    class CafeWithCharities
    {
        public ApplicationUser Cafe { get; set; }

        public List<Charity> Charities { get; set; }
    }
}
