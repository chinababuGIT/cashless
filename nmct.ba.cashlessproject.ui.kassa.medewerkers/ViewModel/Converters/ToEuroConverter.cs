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
    public class ToEuroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string val = (string)value.ToString();
            int ok = val.IndexOf(',');
            if (ok==0)
            {
                return "€ " + val + ",-";
            }
            else
            {
                return "€ " + val;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
