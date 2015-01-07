using nmct.ba.cashlessproject.helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class LongToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long val = (long)value;
            try
            {
                DateTime time = TimeConverter.ToDateTime(val);
                return time.ToString("dd-MM-yyyy hh:mm");
            }
            catch (Exception)
            {
                
            }
            return "";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
