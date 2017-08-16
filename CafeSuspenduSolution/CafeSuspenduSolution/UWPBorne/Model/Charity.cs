using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Model
{
    class Charity
    {
        public int? CharityId { get; set; }
        public int? NbCoffeeOffered { get; set; }
        public int? NbCoffeeConsumed { get; set; }
        public string RowVersion { get; set; }
    }
}
