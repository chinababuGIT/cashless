using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.kassa.Helper;
using nmct.ba.cashlessproject.ui.kassa.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class ApplicationVM : ObservableObject
    {
        public static TokenResponse Token = null;
        public static Organisation CurrentOrganisation = null;
        public static string URL_registerEmployee = "http://localhost:8080/api/registerEmployee";
        public static string URL_log = "http://localhost:8080/api/log";
        public ApplicationVM()
        {


            this.NavigationWidth = new GridLength(0, GridUnitType.Star);
            this.NavigationMinWidth = 0;

            pages = new List<IPage>();

           
            pages.Add(new PageKassaVM());

            //settings instellen
            this.Settings= new Setting(){
                Type=Setting.TypeEnum.bediende,
                RegisterID=1,
                Username="test",
                Password = "2s588k/0kB31nKqs2h696g=="
                
            };
           //get id from name

            //login
            Login();
            
        }

        #region "props"
       

        private IPage currentPage;
        public IPage CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        public object Current { get; set; }
        public RegisterEmployee CurrentRegisterEmployee { get; set; }
       

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
            set { _NavigationWidth = value; OnPropertyChanged("NavigationWidth"); }
        }
        private int _NavigationMinWidth;

        public int NavigationMinWidth
        {
            get { return _NavigationMinWidth; }
            set { _NavigationMinWidth = value; OnPropertyChanged("NavigationMinWidth"); }
        }
       
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        
        #region settings
        private Setting _Settings;

        public Setting Settings
        {
            get { return _Settings; }
            set { _Settings = value; OnPropertyChanged("Settings"); }
        }
        

        #endregion


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




        private void Login()
        {

            ApplicationVM.Token = GetToken();

            if (!ApplicationVM.Token.IsError)
            {
                this.ChangePage(new PageStartVM());
               
                this.NavigationWidth = new GridLength(1, GridUnitType.Star);
                this.NavigationMinWidth = 150;

                App.Current.MainWindow.Height = 600;
                App.Current.MainWindow.Width = 800;
                App.Current.MainWindow.ResizeMode = ResizeMode.CanResize;

            }
            else
            {
                this.Error = "Gebruikersnaam en wachtwoord verkeerd!";
                this.NavigationWidth = new GridLength(1, GridUnitType.Star);
                this.NavigationMinWidth = 150;

                App.Current.MainWindow.Height = 600;
                App.Current.MainWindow.Width = 800;
                App.Current.MainWindow.ResizeMode = ResizeMode.CanResize;
                this.ChangePage(new PageStartVM());
            }
        }

        private TokenResponse GetToken()
        {
            try
            {
                OAuth2Client client = new OAuth2Client(new Uri("http://localhost:8080/token"));
                TokenResponse resp = client.RequestResourceOwnerPasswordAsync(this.Settings.Username, this.Settings.Password).Result;
                return resp;
            }
            catch (Exception ex)
            {

                this.Error = "De server is niet beschikbaar";
                return null;
            }
            
        }

        private void LogOff()
        {

            ApplicationVM.Token = null;



            this.NavigationWidth = new GridLength(0, GridUnitType.Star);
            this.NavigationMinWidth = 0;

            CurrentPage = new PageStartVM();
            App.Current.MainWindow.Height = 250;
            App.Current.MainWindow.Width = 600;
            App.Current.MainWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            App.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
        }

        public async void StartSession()
        {
            Employee currentEmployee = Current as Employee;
            if (currentEmployee!=null)
            {
                int registerID = Settings.RegisterID;
                int employeeID = currentEmployee.ID;

                RegisterEmployee registerEmployee = new RegisterEmployee()
                {
                    RegisterID = registerID,
                    EmployeeID = employeeID,
                    From=TimeConverter.ToUnixTimeStamp(DateTime.Now),
                    Until=TimeConverter.ToUnixTimeStamp(DateTime.Now)
                };
                //toevoegen aan props
                this.CurrentRegisterEmployee = registerEmployee;
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    string json = JsonConvert.SerializeObject(registerEmployee);
                    HttpResponseMessage response = await client.PostAsync(URL_registerEmployee, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        
                        string ex = currentEmployee.Name + " heeft zijn sessie gestart op " + DateTime.Now.ToString("dd-MM-yyy hh:mm");
                        this.LogException(new Exception(ex));
                    }

                } 
            }
           
        }

        public async void EndSession()
        {
            Employee currentEmployee = Current as Employee;
            if (currentEmployee != null)
            {
                int registerID = Settings.RegisterID;
                int employeeID = currentEmployee.ID;

                RegisterEmployee registerEmployee = new RegisterEmployee()
                {
                    RegisterID = registerID,
                    EmployeeID = employeeID,
                    From= this.CurrentRegisterEmployee.From,
                    Until= TimeConverter.ToUnixTimeStamp(DateTime.Now)
                };

                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    string json = JsonConvert.SerializeObject(registerEmployee);
                    HttpResponseMessage response = await client.PutAsync(URL_registerEmployee, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string ex = currentEmployee.Name + " heeft zijn sessie gestopt op " + DateTime.Now.ToString("dd-MM-yyy hh:mm");
                        this.LogException(new Exception(ex));
                    }

                } 
            }
            
        }

        public async void LogException(Exception ex)
        {
            model.Kassa.Log log = new model.Kassa.Log()
            {
                RegisterID=this.Settings.RegisterID,
                Message=ex.Message.ToString(),
                StackTrace=(ex.StackTrace==null)? string.Empty: ex.StackTrace.ToString(),
                Timestamp=TimeConverter.ToUnixTimeStamp(DateTime.Now)
            };
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(log);
                HttpResponseMessage response = await client.PostAsync(URL_log, new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Afhandeling mislukt");
                }
                else
                {
                    if (ex.StackTrace != null && ex.Message!="Object reference not set to an instance of an object.")
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                   
                }
            }
        }

        #endregion

        
    }
}
