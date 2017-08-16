using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Model
{
    class ApplicationUser
    {
        public string CafeName { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public List<TimeTable> TimeTables { get; set; }
    }
}
