using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Borne2017.Converter
{
    //Sert à donner le format pour les heures.
    public class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;

            if (parameter == null)
                return null;

            //Si aucune selection 
            if((int)((TimeSpan)value).Days == 1)
            {
                return "";
            }

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
