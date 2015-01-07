using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageLoginVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public PageLoginVM()
        {
            
        }
        #region props
        public string Name
        {
            get { return "Login"; }
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

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; OnPropertyChanged("Username"); }
        }
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged("Password"); }
        }
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged("Error"); }
        }
#endregion

        #region commands
        public ICommand LoginCommand
        {
            get { return new RelayCommand<object>(Login); }
        }
        #endregion

        #region functions

        private async void LoginOLD()
        {
            string username = this.Username;
            string password = this.Password;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage respone = await client.GetAsync(new Uri("http://localhost:8080/api/organisations?username="+ username + "&password=" + password));
                if (respone.IsSuccessStatusCode)
                {
                    string content = await respone.Content.ReadAsStringAsync();
                    Organisation result = JsonConvert.DeserializeObject<Organisation>(content);
                  

                    if (result != null)
                    {
                      
                        appvm.ChangePage(new PageStartVM());
                        appvm.NavigationWidth = new GridLength(1, GridUnitType.Star);
                        appvm.NavigationMinWidth = 150;
                      
                    }
                    else
                    {

                    }
                }
               
            }
        }

        private void Login(object obj)
        {
            PasswordBox passwordBox = obj as PasswordBox;
            string password = passwordBox.Password;
            this.Password = password;
            this.Password = Cryptography.Encrypt(this.Password);
            ApplicationVM.Token = GetToken();

            if (!ApplicationVM.Token.IsError)
            {
                GetOrganisation();

                appvm.ChangePage(new PageStartVM());
                appvm.NavigationWidth = new GridLength(1, GridUnitType.Star);
                appvm.NavigationMinWidth = 150;
               
                
                App.Current.MainWindow.Height=600;
                App.Current.MainWindow.Width=800;
                App.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                this.Error = "Gebruikersnaam en wachtwoord verkeerd!";
            }
        }

      

        private TokenResponse GetToken()
        {
            try
            {
                OAuth2Client client = new OAuth2Client(new Uri("http://localhost:8080/token"));
                TokenResponse resp = client.RequestResourceOwnerPasswordAsync(this.Username, this.Password).Result;
                return resp;
            }
            catch (Exception ex)
            {

                this.Error = "De server is offline";
                return null;
            }
            
        }

        private async void GetOrganisation()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri("http://localhost:8080/api/organisation"));

                HttpResponseMessage response = await task;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    
                    ApplicationVM.CurrentOrganisation = JsonConvert.DeserializeObject<Organisation>(json);

                }
            }
        }
        #endregion


    }
}
