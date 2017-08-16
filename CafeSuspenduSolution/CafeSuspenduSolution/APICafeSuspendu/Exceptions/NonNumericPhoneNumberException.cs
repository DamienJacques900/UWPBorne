using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APICafeSuspendu.Exceptions
{
    public class NonNumericPhoneNumberException : Exception
    {
        public string ErrorMessage { get; set; }

        public NonNumericPhoneNumberException()
        {
            ErrorMessage = "The phone number provided contains non-numeric characters.";
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}