using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borne2017.Model
{
    public class ApplicationUser
    {
        public String CafeName { get; set; }
        public String Street { get; set; }
        public String Number { get; set; }
        public List<TimeTable> TimeTables { get; set; }
    }
}
