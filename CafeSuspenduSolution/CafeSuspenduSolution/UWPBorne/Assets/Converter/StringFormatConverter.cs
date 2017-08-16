using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace UWPBorne.Converter
{
    class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //No value provided
            if (value == null)
                return null;

            //No format provided
            if (parameter == null)
                return value;


            if ((int)((TimeSpan)value).Days == 1)
                return "";

            return String.Format((string)parameter,
               (int)((TimeSpan)value).TotalHours,
                    ((TimeSpan)value).Minutes);

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
