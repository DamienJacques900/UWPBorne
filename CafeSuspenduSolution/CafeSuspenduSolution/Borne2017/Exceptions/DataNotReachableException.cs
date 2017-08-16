using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPBorne.Exceptions
{
    class DataNotReachableException : Exception
    {
        public string ErrorMessage { get; set; }

        public DataNotReachableException()
        {
            ErrorMessage = "Erreur lors de la tentative de rapatriement des données";
        }

        public string GetMessage()
        {
            return ErrorMessage;
        }
    }
}
