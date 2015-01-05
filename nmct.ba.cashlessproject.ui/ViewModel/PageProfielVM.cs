using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageProfielVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/organisation";
        public PageProfielVM()
        {
            GetOrganisation();
            appvm.SelectPage("");
            this.Error = "";
        }

        #region props
        public string Name
        {
            get { return "Profiel"; }
        }
        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
        public BitmapImage Image
        {
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/star-icon.png", UriKind.Relative)); }
        }

        private Organisation _Organisation;

        public Organisation Organisation
        {
            get { if (_Organisation==null) return new Organisation(); return _Organisation; }
            set { _Organisation = value; OnPropertyChanged("Organisation"); OnPropertyChanged("Update"); }
        }
        private string _Error;

        public string Error
        {
            get { return _Error;}
            set { _Error = value; OnPropertyChanged("Error"); }
        }
        private bool _IsEnabled;

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set { _IsEnabled = value; OnPropertyChanged("IsEnabled"); }
        }
        

        #endregion

        #region commands
        public ICommand Update
        {
            get { return new RelayCommand(UpdateOrganisation); }

        }

        
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelOrganisation); }
        }
        public ICommand PasswordChangedCommand
        {
            get { return new RelayCommand<EventArgs>(PasswordChanged); }
        }
        public ICommand PasswordChangedCommand2
        {
            get { return new RelayCommand<EventArgs>(PasswordChanged2); }
        }

        private void PasswordChanged(EventArgs obj)
        {
            RoutedEventArgs r = obj as RoutedEventArgs;
            PasswordBox box = r.Source as PasswordBox;
            this.Organisation.Password = box.Password;
            if (this.Organisation.Password != this.Organisation.RepeatPassword)
            {
                this.Error = "Wachtwoorden komen niet overeen";
                this.IsEnabled = false;
                OnPropertyChanged("Error");

            }
            else
            {
                this.Error = "";
                this.IsEnabled = true;
                OnPropertyChanged("Error");
            }
        }
        private void PasswordChanged2(EventArgs obj)
        {
            RoutedEventArgs r = obj as RoutedEventArgs;
            PasswordBox box = r.Source as PasswordBox;
            this.Organisation.RepeatPassword = box.Password;
            if (this.Organisation.Password!=this.Organisation.RepeatPassword)
            {
                this.Error = "Wachtwoorden komen niet overeen";
                this.IsEnabled = false;
                OnPropertyChanged("Error");
            }
            else
            {
                this.Error = "";
                this.IsEnabled = true;
                OnPropertyChanged("Error");
            }
        }
        
        //EID COMMANDS
       



        #endregion

        #region functions
        private void CancelOrganisation()
        {
            appvm.ChangePage(new PageStartVM());
        }
        private async void UpdateOrganisation()
        {
            if(this.Organisation.Password==this.Organisation.RepeatPassword){
                using (HttpClient client = new HttpClient())
                {
                    this.Organisation.Password = Cryptography.Encrypt(this.Organisation.Password);
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    string json = JsonConvert.SerializeObject(this.Organisation);
                    HttpResponseMessage response = await client.PutAsync(URL, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        appvm.ChangePage(new PageStartVM());
                    }
                }
            }   
            
        }

       
        private async void GetOrganisation()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL));

                HttpResponseMessage response = await task;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    this.Organisation = null;
                    this.Organisation = JsonConvert.DeserializeObject<Organisation>(json);
                   
                }
            }
        }

        #endregion



    
    }
}
