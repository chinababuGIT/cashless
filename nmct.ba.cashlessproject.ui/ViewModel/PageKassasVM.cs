using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageKassasVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/register";

        public PageKassasVM()
        {
            if (ApplicationVM.Token != null)
            {
                GetRegisters();
                GetRegisterEmployees();
                
            }
        }

        #region props
        public string Name
        {
            get { return "Kassas"; }
        }
        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged("Error"); }
        }
        
        public BitmapImage Image
        {
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/register.png", UriKind.Relative)); }
        }
        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }
        private ObservableCollection<RegisterEmployee> _RegisterEmployees;

        public ObservableCollection<RegisterEmployee> RegisterEmployees
        {
            get { return _RegisterEmployees; }
            set { _RegisterEmployees = value; OnPropertyChanged("RegisterEmployees"); OnPropertyChanged("RegisterEmployeesFilter"); }
        }
 

        public ObservableCollection<RegisterEmployee> RegisterEmployeesFilter
        {
            get { return GetRegisterById(SelectedID); }
           
        }


        private ObservableCollection<Register> _Registers;

        public ObservableCollection<Register> Registers
        {
            get { return _Registers; }
            set { _Registers = value; OnPropertyChanged("Registers");
                   
            }
        }
        private Register _SelectedID;

        public Register SelectedID
        {
            get { return _SelectedID; }
            set
            {
                _SelectedID = value; OnPropertyChanged("SelectedID"); OnPropertyChanged("RegisterEmployeesFilter");
            
            }
        }
        
        

        #endregion

        #region commands
       

        #endregion

        #region functions

        private async void GetRegisterEmployees()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri("http://localhost:8080/api/registeremployee"));

                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));

                HttpResponseMessage response = await task;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<RegisterEmployee> lijst = JsonConvert.DeserializeObject<List<RegisterEmployee>>(json).OrderBy(r=>r.From).ToList<RegisterEmployee>();
                    this.RegisterEmployees = new ObservableCollection<RegisterEmployee>(lijst);
                    if (this.RegisterEmployees.Count == 0)
                    {
                        this.Error = "Geen gegevens beschikbaar";
                    }
                }

              
            }

        }
        private async void GetRegisters()
        {
            List<Register> lijst=new List<Register>();
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(new Uri(URL));
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    lijst = JsonConvert.DeserializeObject<List<Register>>(json);

                    if (lijst.Count==0)
                    {
                        this.Error = "Geen kassas's beschikbaar";
                    }

                    lijst.Add(new Register()
                    {
                        ID=-1,
                        Name="All"
                    });
                    //sort

                    lijst=lijst.OrderBy(a=>a.ID).ToList<Register>();
                    ObservableCollection<Register> sortedList = new ObservableCollection<Register>(lijst);
                    this.Registers = sortedList;

                    this.SelectedID = sortedList[0];

                }
            }



        }


        private ObservableCollection<RegisterEmployee> GetRegisterById(Register reg)
        {
            if (reg != null && reg.ID!=-1)
            {

                List<RegisterEmployee> lijst = this.RegisterEmployees.Where(r => r.RegisterID == reg.ID).ToList<RegisterEmployee>();
                ObservableCollection<RegisterEmployee> o = new ObservableCollection<RegisterEmployee>(lijst);

               
                return o;
            }
            else
            {
                return this.RegisterEmployees;
            }
        }
       

        #endregion

    }
}
