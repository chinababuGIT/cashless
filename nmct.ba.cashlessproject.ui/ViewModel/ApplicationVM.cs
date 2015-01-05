using GalaSoft.MvvmLight.CommandWpf;
using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
   public class ApplicationVM : ObservableObject
    {
        public static TokenResponse Token=null;
        public static Organisation CurrentOrganisation = null;

        public ApplicationVM()
        {
            

            this.NavigationWidth = new GridLength(0,GridUnitType.Star);
            this.NavigationMinWidth = 0;

            pages = new List<IPage>();
          
            pages.Add(new PageStartVM());
            Pages.Add(new PageProductenVM());
            pages.Add(new PageMedewerkersVM());
            pages.Add(new PageKassasVM());
            pages.Add(new PageKlantenVM());
            pages.Add(new PageStatistiekenVM());

            //pages.Add(new PageKeuzeVM());
            //pages.Add(new PageBetalingVM());
            //pages.Add(new PageProfielVM());        
            //pages.Add(new PageLoginVM());
            // Add other pages

            CurrentPage = new PageLoginVM();
            App.Current.MainWindow.Height=270;
            App.Current.MainWindow.Width=600;
            App.Current.MainWindow.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            App.Current.MainWindow.ResizeMode=ResizeMode.NoResize;
       
            //currentPage = Pages[0];
        }
       
        #region "props"
       
        private IPage currentPage;
        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        public object Current { get; set; }

        private List<IPage> pages;
        public List<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new List<IPage>();
                return pages;
            }
        }
        private GridLength _NavigationWidth;

        public GridLength NavigationWidth
        {
            get { return _NavigationWidth; }
            set {  _NavigationWidth = value; OnPropertyChanged("NavigationWidth"); }
        }
        private int _NavigationMinWidth;

        public int NavigationMinWidth
        {
            get { return _NavigationMinWidth; }
            set { _NavigationMinWidth = value; OnPropertyChanged("NavigationMinWidth"); }
        }

       
        
        #endregion

        #region commands
        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }
        public ICommand LogOffCommand
        {
            get { return new RelayCommand(LogOff); }
        }
        public ICommand ChangePasswordCommand
        {
            get { return new RelayCommand(ChangePassword); }
        }

        #endregion

        #region functions
        public void ChangePage(IPage page)
        {
            SelectPage(page.Name);
            page.Selected = true;
            CurrentPage = page;

        }

        public int SelectPage(string name)
        {
            int iGetal = -1;
            for (int i = 0; i < pages.Count; i++)
            {
                if (pages[i].Name == name)
                {
                    pages[i].Selected = true;
                    iGetal = i;
                }
                else
                {
                    pages[i].Selected = false;
                }
            }
            return iGetal;
        }


 
        private void LogOff()
        {

            ApplicationVM.Token = null;

            

            this.NavigationWidth = new GridLength(0, GridUnitType.Star);
            this.NavigationMinWidth = 0;

            ChangePage(new PageLoginVM());
            App.Current.MainWindow.Height = 250;
            App.Current.MainWindow.Width = 600;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            App.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
        }
        private void ChangePassword()
        {
            ChangePage(new PageProfielVM());
        }

       
        #endregion


    }
}
