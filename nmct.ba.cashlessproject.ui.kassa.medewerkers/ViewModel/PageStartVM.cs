using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.kassa.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class PageStartVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL_employee = "http://localhost:8080/api/employee";
        public static string URL_registers = "http://localhost:8080/api/register";
        public static string URL_customers = "http://localhost:8080/api/customer";

        public PageStartVM()
        {
            if (ApplicationVM.Token != null && appvm!=null)
            {
                if (appvm.Error!="")
                {
                    this.Error = appvm.Error;
                }
                if (appvm.Settings.Type==Setting.TypeEnum.bediende)
                {                
                    this.NewSessionContent = "Nieuwe sessie starten";
                    Task<bool> task=GetEmployees();
                    task.Wait();
                    this.Overlay_bediende = true;
                }
                if (appvm.Settings.Type == Setting.TypeEnum.klant)
                {
                    this.NewSessionContent = "Ga door";
                    this.Overlay_klant = true;
                }
            }

        }

        #region props
        public string Name
        {
            get { return "Start"; }
        }

        private bool _Selected;
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyChanged("Selected"); }
        }
        public BitmapImage Image
        {
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/product.png", UriKind.Relative)); }
        }

       

        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }
        private bool _Overlay_bediende;

        public bool Overlay_bediende
        {
            get { return _Overlay_bediende; }
            set { _Overlay_bediende = value; OnPropertyChanged("Overlay_bediende"); }
        }
        private bool _Overlay_klant;

        public bool Overlay_klant
        {
            get { return _Overlay_klant; }
            set { _Overlay_klant = value; OnPropertyChanged("Overlay_klant"); }
        }
        private ObservableCollection<Employee> _Employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private ObservableCollection<Customer> _Customers;

        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set { _Customers = value; OnPropertyChanged("Customers"); }
        }
        private Employee _SelectedEmployee;

        public Employee SelectedEmployee
        {
            get {  return _SelectedEmployee; }
            set { _SelectedEmployee = value; OnPropertyChanged("SelectedEmployee"); }
        }
        private string _NewSessionContent;

        public string NewSessionContent
        {
            get { return _NewSessionContent; }
            set { _NewSessionContent = value; OnPropertyChanged("NewSessionContent"); }
        }
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged("Error"); }
        }
        
        


        #endregion

        #region commands

        public ICommand NewSessionCommand
        {
            get { return new RelayCommand(NewSession); }
        }

       
       




        #endregion

        #region functions


      
        private void NewSession()
        {

            if (appvm.Settings.Type == Setting.TypeEnum.bediende)
            {
               
                if (this.SelectedEmployee!=null && this.SelectedEmployee.ID!=0)
                {
                    appvm.Current = SelectedEmployee;
                    //sessie starten
                    #region kassa controle
                    Task<bool> task=GetRegisters(); //controle of kassa bestaaat
                    task.Wait();
                    if (task.IsCompleted)
                    {
                        if (task.Result==true)
                        {
                            appvm.StartSession();
                            appvm.ChangePage(new PageKassaVM());
                        }
                        else
                        {
                            this.Error = "Deze kassa bestaat niet meer";
                        }
                    }
                    #endregion

                }
                else
                {
                    this.Error = "Geen gebruikers aanwezig";
                    appvm.ChangePage(new PageStartVM());
                   
                }
                     
            }
            if (appvm.Settings.Type == Setting.TypeEnum.klant)
            {
                //sessie db
                EID eid = EIDReader.LaadEID();
                if (eid!=null)
                {
                    #region kassa controle
                    try
                    {
                    Task<bool> task = GetRegisters(); //controle of kassa bestaaat
                    Task<bool> taskCustomers = GetCustomers();
                    taskCustomers.Wait();
                    task.Wait();
                    if (task.IsCompleted && taskCustomers.IsCompleted)
                    {
                        if (task.Result == true)
                        {
                            if (eid.Error == "" || eid.Error == null)
                            {
                                Customer customer = GetCustomerFromRegisterNumber(eid);
                                if (customer != null)
                                {
                                    appvm.Current = customer;
                                    appvm.ChangePage(new PageKassaVM_klant());

                                }
                                else
                                {
                                    appvm.Current = eid;
                                    appvm.ChangePage(new PageKassaVM_klant_add());
                                }

                            }
                            else
                            {
                                this.Error = eid.Error;
                            }


                        }
                        else
                        {
                            this.Error = "Deze kassa bestaat niet meer";
                        }
                    }
                    }
                    catch (Exception ex)
                    {

                        this.Error = "De login is verkeerd gelopen";
                    }
                    
                   
                    #endregion

                    
                   
                }
                else
                {
                    appvm.Current = null;
                   
                    this.Error = "De kaartlezer is niet aangesloten";
                }
               
            }


        }

        private async Task<bool> GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL_employee));

                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));
                task.Wait();
                HttpResponseMessage response = task.Result;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Employee> lijst = JsonConvert.DeserializeObject<List<Employee>>(json).Where(r => r.Hidden == false).ToList<Employee>();

                    if (lijst.Count == 0) lijst.Add(new Employee()
                    {
                        Name = "Geen gebruikers",
                        ID = 0

                    });
                    this.Employees = new ObservableCollection<Employee>(lijst);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task =  client.GetAsync(new Uri(URL_registers));

                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));
                task.Wait();
                HttpResponseMessage response = task.Result;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Register> lijst = JsonConvert.DeserializeObject<List<Register>>(json).Where(r => r.ID == appvm.Settings.RegisterID).ToList<Register>();
                    if (lijst.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private async Task<bool> GetCustomers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL_customers));

                task.Wait();
                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));

                HttpResponseMessage response = await task;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    this.Customers = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                    return true;

                }
            }
            return false;

        }
        private Customer GetCustomerFromRegisterNumber(EID eid)
        {
            if (this.Customers != null)
            {
                for (int i = 0; i < this.Customers.Count; i++)
                {
                    if (this.Customers[i].RegisterNumber == eid.RegisterNumber)
                    {
                        return this.Customers[i];
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
