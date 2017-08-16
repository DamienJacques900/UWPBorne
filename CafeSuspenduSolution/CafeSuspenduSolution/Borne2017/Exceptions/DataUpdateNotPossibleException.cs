using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Exceptions
{
    class DataUpdateNotPossibleException : Exception
    {
        public string ErrorMessage { get; set; }

        public DataUpdateNotPossibleException()
        {
            ErrorMessage = "Impossible de mettre à jour les données. Veuillez réessayer plus tard.";
        }

        public string GetMessage()
        {
            return ErrorMessage;
        }
    }
}
