using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public interface IPage
    {
        string Name { get; }
        bool Selected { get; set; }
        
        BitmapImage Image { get; }

    }
}
