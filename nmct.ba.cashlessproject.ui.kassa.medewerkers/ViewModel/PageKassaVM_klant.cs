using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.kassa.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class PageKassaVM_klant: ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/product";
        public static string URL_customers = "http://localhost:8080/api/customer";
        public static string URL_registers = "http://localhost:8080/api/register";
        public static string URL_sales = "http://localhost:8080/api/sale";
        private const int MAX = 100;
        public PageKassaVM_klant()
        {
            if (ApplicationVM.Token!=null)
            {
                if (GetCustomers().IsCompleted && GetRegisters().IsCompleted)
                {
                    GetMoney();
                    if (appvm!=null && appvm.Current!=null)
                    {
                      
                        this.SelectedCustomer=appvm.Current as Customer;

                       
                    }

                }
                else
                {
                    appvm.ChangePage(new PageStartVM());
                }
            }
            
        }

       

       

        #region props
        public string Name
        {
            get { return "Producten"; }
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

        private ObservableCollection<Product> _Products;

        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set { _Products = value; OnPropertyChanged("Products"); }
        }

        private ObservableCollection<Customer> _Customers;

        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set {_Customers = value; OnPropertyChanged("Customers"); }
        }
       
        private ObservableCollection<Register> _Registers;

        public ObservableCollection<Register> Registers
        {
            get { return _Registers; }
            set { _Registers = value; OnPropertyChanged("Registers"); }
        }

        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }


        private Customer _SelectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set {  _SelectedCustomer = value; OnPropertyChanged("SelectedCustomer"); }
        }
        
       

        private Register _SelectedRegister;

        public Register SelectedRegister
        {
            get { return _SelectedRegister; }
            set { _SelectedRegister = value; OnPropertyChanged("SelectedRegister"); }
        }
        private ObservableCollection<int> _Money;

        public ObservableCollection<int> Money
        {
            get { return _Money; }
            set { _Money = value; OnPropertyChanged("Money"); }
        }
        public double Sum
        {
            get {if(SelectedCustomer_New!=null) return Math.Abs(SelectedCustomer_New.Balance - SelectedCustomer.Balance);
            return 0;
            }

        }
        #region telling

        private Customer _SelectedCustomer_New;

        public Customer SelectedCustomer_New
        {
            get { return _SelectedCustomer_New; }
            set { _SelectedCustomer_New = value; OnPropertyChanged("SelectedCustomer_New"); OnPropertyChanged("Sum"); }
        }
        private double _Change;

        public double Change
        {
            get { return _Change; }
            set { _Change = value; OnPropertyChanged("Change"); }
        }
        #endregion
        
        private bool _Overlay_pay;

        public bool Overlay_pay
        {
            get { return _Overlay_pay; }
            set { _Overlay_pay = value; OnPropertyChanged("Overlay_pay"); }
        }
       
        
#endregion  
        
        #region commands

        #region telling
        public ICommand PlusCommand
        {
            get { return new RelayCommand<int>(Plus); }
        }

        public ICommand DigitCommand
        {
            get { return new RelayCommand<string>(Digit); }
        }
        #endregion
       
        public ICommand TotalCommand
        {
            get { return new RelayCommand(Total); }
        }

       
        public ICommand PayCommand
        {
            get { return new RelayCommand(Pay); }
        }
        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }
        public ICommand EndSessionCommand
        {
            get { return new RelayCommand(EndSession); }
        }

        #endregion

        #region functions

        private void Total(){
            try
            {
                if (this.Change == MAX || this.Change == 0)
                {

                    throw new Exception("Het bedrag moet verschillend zijn van 0 of" + MAX.ToString());
                }
                else
                {
                    if (this.SelectedCustomer_New.Balance <= this.SelectedCustomer.Balance)
                    {
                        throw new Exception("Je kan niet minder opladen");
                    }
                    else
                    {
                        this.Overlay_pay = true;
                    }
                   
                }
            }
            catch (Exception ex )
            {

                appvm.LogException(ex);
            }
           
            
        }

        private void Cancel()
        {
            this.Overlay_pay = false;
        }
         private void EndSession()
        {
            appvm.ChangePage(new PageStartVM());
        }


        #region telling
        bool isKommaGetal = false;

        //euro briefjes
        private void Plus(int obj)
        {
            Customer selected;
            if (SelectedCustomer_New == null)
            {
                selected = new Customer()
                {
                    ID=this.SelectedCustomer.ID,
                    Address=this.SelectedCustomer.Address,
                    Balance=this.SelectedCustomer.Balance,
                    Name=this.SelectedCustomer.Name,
                    SurName=this.SelectedCustomer.SurName,
                    Hidden=this.SelectedCustomer.Hidden,
                    Picture=this.SelectedCustomer.Picture,
                    RegisterNumber=this.SelectedCustomer.RegisterNumber
                };
                    
            }
            else
            {
                selected = this.SelectedCustomer_New;
            }
            
            double tussenBalance = selected.Balance;

            selected.Balance+=obj;
            if (selected.Balance > MAX) selected.Balance = MAX;
           
            this.SelectedCustomer_New = null;
            this.SelectedCustomer_New = selected;
            //change laden
            this.Change = Math.Abs(this.SelectedCustomer.Balance - this.SelectedCustomer_New.Balance); 
            OnPropertyChanged("Change");
        }
       
        //letters
        private void Digit(string getal)
        {
            try
            {
                int _getal;
                Customer selected;
                bool isGetal = Int32.TryParse(getal, out _getal);



                if (this.SelectedCustomer_New == null)
                {
                    selected = new Customer()
                    {
                        ID = this.SelectedCustomer.ID,
                        Address = this.SelectedCustomer.Address,
                        Balance = 0,
                        Name = this.SelectedCustomer.Name,
                        SurName=this.SelectedCustomer.SurName,
                        Hidden=this.SelectedCustomer.Hidden,
                        Picture = this.SelectedCustomer.Picture,
                        RegisterNumber = this.SelectedCustomer.RegisterNumber
                    };
                }
                else
                {
                    selected = this.SelectedCustomer_New;
                }





                if (isGetal)
                {
                    //als het een getal is
                    string getal_string = selected.Balance.ToString();
                    if (getal_string == "0")
                    {
                        getal_string = _getal.ToString();
                    }
                    else
                    {
                        //als komma word gebruikt
                        if (isKommaGetal == true)
                        {
                            getal_string += ",";
                            getal_string += _getal.ToString();
                            isKommaGetal = false;
                        }
                        else
                        {
                            getal_string += _getal.ToString();
                        }

                    }

                    selected.Balance = Double.Parse(getal_string, NumberStyles.Currency, NumberFormatInfo.CurrentInfo);
                }
                else
                {
                    if (getal == "c")
                    {
                        isKommaGetal = false;
                        string getal_string = selected.Balance.ToString();
                        getal_string = getal_string.Substring(0, getal_string.Length - 1);
                        if (getal_string == "") getal_string = "0";
                        selected.Balance = Double.Parse(getal_string, NumberStyles.Currency, NumberFormatInfo.CurrentInfo);
                        //appvm.LogException(new Exception("Er werd op de c gedrukt"));
                    }
                    if (getal == ".")
                    {
                        isKommaGetal = true;

                    }
                }
                if (selected.Balance > MAX)
                {
                    selected.Balance = MAX;

                    appvm.LogException(new Exception("Bedrag is groter dan " + MAX.ToString()));
                }
                if (selected.Balance <= this.SelectedCustomer.Balance)
                {
                    //selected.Balance = 0;

                    //appvm.LogException(new Exception("Bedrag is kleiner dan " + MAX.ToString()));
                }
                this.SelectedCustomer_New = null;
                this.SelectedCustomer_New = selected;
                //change laden
                this.Change = Math.Abs(this.SelectedCustomer.Balance - this.SelectedCustomer_New.Balance);
                OnPropertyChanged("Change");

            }
            catch (Exception ex)
            {
                appvm.LogException(ex);

            }
            finally
            {

            }

        }
        
       
        #endregion

        #region get data
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
        private async Task<bool> GetRegisters()
        {
            List<Register> lijst = new List<Register>();
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL_registers));
                task.Wait();
                HttpResponseMessage response= await task;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    this.Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                    
                    
                    SelectedRegister = this.Registers[1];
                    return true;
                }
            }
            return false;


        }

        private void GetMoney()
        {
            this.Money = new ObservableCollection<int>();

            this.Money.Add(1);
            this.Money.Add(5);
            this.Money.Add(10);
            this.Money.Add(20);
            this.Money.Add(50);

        }

        #endregion


        private int GetIdIn(ObservableCollection<Product> lijst, Product product)
        {
            for (int i = 0; i < lijst.Count; i++)
            {
                if (lijst[i].ID == product.ID)
                {
                    return i;
                }
            }

            return -1;
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

      



        private async void Pay()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    

                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    string json = JsonConvert.SerializeObject(this.SelectedCustomer_New);
                    HttpResponseMessage response = await client.PutAsync(URL_customers, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        //ok
                        this.Overlay_pay = false;
                        this.Customers = null;
                        await GetCustomers();

                        double verschil= this.SelectedCustomer_New.Balance - this.SelectedCustomer.Balance;
                        string ex = this.SelectedCustomer_New.Name + " heeft " + verschil.ToString() + " euro opgeladen";
                        appvm.LogException(new Exception(ex));

                        this.SelectedCustomer = null;
                        this.SelectedCustomer = GetCustomerFromRegisterNumber(appvm.Current as EID);
                        this.SelectedCustomer_New = null;
                    }
                    else
                    {
                        throw new Exception("Betaling is mislukt"); 
                    }

                }
            }
            catch (Exception ex )
            {

                appvm.LogException(ex);
                appvm.ChangePage(new PageStartVM());
            }
            
        }

        #endregion
    
    }
}
