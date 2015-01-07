using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageStartVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

        public string Name
        {
            get { return (ApplicationVM.CurrentOrganisation==null)? "Home":ApplicationVM.CurrentOrganisation.Name; }
        }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
        public BitmapImage Image
        {
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/home.png", UriKind.Relative)); }
        }

        #region commands

        public ICommand GoCommand
        {
            get { return new RelayCommand<string>(Go); }
        }

       
        #endregion

        private void Go(string soort)
        {
            switch (soort)
            {
                case "Producten": appvm.ChangePage(new PageProductenVM());
                    break;
                case "Producten_add": appvm.ChangePage(new PageProductenVM_add());
                    break;
                case "Klanten": appvm.ChangePage(new PageKlantenVM());
                    break;
                case "Klanten_add": appvm.ChangePage(new PageKlantenVM_add());
                    break;
                case "Medewerkers": appvm.ChangePage(new PageMedewerkersVM());
                    break;
                case "Medewerkers_add": appvm.ChangePage(new PageMedewerkersVM_add());
                    break;
                case "Profiel": appvm.ChangePage(new PageProfielVM());
                    break;
                case "Statistieken": appvm.ChangePage(new PageStatistiekenVM());
                    break;
                default: appvm.ChangePage(new PageStartVM());
                    break;
            }
        }
    }
}
