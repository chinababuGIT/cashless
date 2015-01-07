using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class PageKassaVM: ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/product";
        public static string URL_customers = "http://localhost:8080/api/customer";
        public static string URL_registers = "http://localhost:8080/api/register";
        public static string URL_sales = "http://localhost:8080/api/sale";
        private const int MAX = 100;
        public PageKassaVM()
        {
            if (ApplicationVM.Token!=null)
            {       
                if (GetData() && appvm.Settings.RegisterID!=0)
                {
                    this.SelectedEmployee = appvm.Current as Employee;

                    List<Register> lijst = this.Registers.Where(r => r.ID == appvm.Settings.RegisterID).ToList<Register>();
                   
                    this.SelectedRegister = lijst.First();
                    this.Overlay_change = false;
                    this.EID_visible = true;

                    //
                    

                    this.Customers_visible = false;
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
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }
        private EID _EID;

        public EID EID
        {
            get { return _EID; }
            set { _EID = value; OnPropertyChanged("EID"); }
        }
        private ObservableCollection<Product> _Basket;

        public ObservableCollection<Product> Basket
        {
            get { return _Basket; }
            set { _Basket = value; OnPropertyChanged("Basket"); OnPropertyChanged("Sum"); }
        }

        public double Sum
        {
            get { return CalculateTotal(this.Basket); }
           
        }
        
       

        private Customer _SelectedCustomer;

        public Customer SelectedCustomer
        {
            get
            {
                if (_SelectedCustomer==null)
                {
                    return new Customer() { Balance = 0,Name="..." };
                }
                
                return _SelectedCustomer; }
            set { _SelectedCustomer = value; OnPropertyChanged("SelectedCustomer");}
        }
        private Employee _SelectedEmployee;

        public Employee SelectedEmployee
        {
            get { return _SelectedEmployee; }
            set
            {
                _SelectedEmployee = value; OnPropertyChanged("SelectedEmployee");

            }
        }
        private Register _SelectedRegister;

        public Register SelectedRegister
        {
            get { return _SelectedRegister; }
            set { _SelectedRegister = value; OnPropertyChanged("SelectedRegister"); }
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

        #region visibility
        private bool _Overlay_change;

        public bool Overlay_change
        {
            get { return _Overlay_change; }
            set { _Overlay_change = value; OnPropertyChanged("Overlay_change"); }
        }
        private bool _Overlay_pay;

        public bool Overlay_pay
        {
            get { return _Overlay_pay; }
            set { _Overlay_pay = value; OnPropertyChanged("Overlay_pay"); }
        }
        private bool _Customers_visible;

        public bool Customers_visible
        {
            get { return _Customers_visible; }
            set { _Customers_visible = value; OnPropertyChanged("Customers_visible"); }
        }
        private bool _EID_visible;

        public bool EID_visible
        {
            get { return _EID_visible; }
            set { _EID_visible = value; OnPropertyChanged("EID_visible"); }
        }
        #endregion

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
        public ICommand EIDCommand
        {
            get { return new RelayCommand(LoadEID); }
        }
        public ICommand AddToBasketCommand
        {
            get { return new RelayCommand<Product>(AddToBasket); }
        }
        public ICommand AddAmountCommand
        {
            get { return new RelayCommand<Product>(AddAmount); }
        }

       
        public ICommand RemoveAmountCommand
        {
            get { return new RelayCommand<Product>(RemoveAmount); }
        }

       
        public ICommand PayCommand
        {
            get { return new RelayCommand(Pay); }
        }
        public ICommand CancelCommand
        {
            get { return new RelayCommand(Cancel); }
        }
        public ICommand NewCommand
        {
            get { return new RelayCommand(New); }
        }

        public ICommand EndSessionCommand
        {
            get { return new RelayCommand(EndSession);}
        }

       
       
        public ICommand TotalCommand
        {
            get { return new RelayCommand(Total); }
        }

        
        public ICommand EditBalanceCommand
        {
            get { return new RelayCommand(EditBalance); }
        }
        public ICommand SaveBalanceCommand
        {
            get { return new RelayCommand(SaveBalance); }
        }
        public ICommand CancelBalanceCommand
        {
            get { return new RelayCommand(CancelBalance); }
        }

        
      
       
       
       

        #endregion

        #region functions
        private void AddToBasket(Product product)
        {
            AddAmount(product);
        }

        #region telling
        bool isKommaGetal = false;

        //euro briefjes
        private void Plus(int obj)
        {
            try
            {
                if (this.SelectedCustomer == null)
                {
                    System.Windows.Forms.MessageBox.Show("Geen gebruiker geselecteerd");
                    return;
                }
                Customer selected;
                if (SelectedCustomer_New == null)
                {
                    selected = new Customer()
                    {
                        ID = this.SelectedCustomer.ID,
                        Address = this.SelectedCustomer.Address,
                        Balance = this.SelectedCustomer.Balance,
                        Name = this.SelectedCustomer.Name,
                        Picture = this.SelectedCustomer.Picture,
                        RegisterNumber = this.SelectedCustomer.RegisterNumber
                    };

                }
                else
                {
                    selected = this.SelectedCustomer_New;
                }

                double tussenBalance = selected.Balance;

                selected.Balance += obj;
                if (selected.Balance > MAX) selected.Balance = MAX;
                this.SelectedCustomer_New = null;
                this.SelectedCustomer_New = selected;
                //change laden
                this.Change = Math.Abs(this.SelectedCustomer.Balance - this.SelectedCustomer_New.Balance);
                OnPropertyChanged("Change");
            }
            catch (Exception)
            {

                
            }
            
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
                        appvm.LogException(new Exception("Er werd op de c gedrukt"));
                    }
                    if (getal == ".")
                    {
                        isKommaGetal = true;

                    }
                }
                if (selected.Balance > MAX)
                {
                    selected.Balance = MAX;

                    appvm.LogException(new Exception("Bedrag is groter dan "+ MAX.ToString()));
                    //throw new Exception("Bedrag is groter dan 100");
                }
                if (selected.Balance <= this.SelectedCustomer.Balance)
                {
                    //selected.Balance = 0;

                    appvm.LogException(new Exception("Bedrag is kleiner dan "+ MAX.ToString()));
                    //throw new Exception("Bedrag is kleiner dan origneel");
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

        

       

       


       

        private bool IsLimietOk(ObservableCollection<Product> lijst,Product future)
        {
            try
            {
                double totaal = CalculateTotal(lijst);
                //future berekening
                totaal += future.Price;

                if (this.SelectedCustomer != null)
                {
                    if (totaal > this.SelectedCustomer.Balance)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Er is iets foutgelopen bij de limietcontrole",ex);              
            }

            return true;
           
        }

        private void EditBalance()
        {
            try
            {
                if (SelectedCustomer != null)
                {
                    if (this.SelectedCustomer.Balance==0 && this.SelectedCustomer.ID==0)
                    {
                        throw new Exception("De balans kan je niet aanpassen");
                    }
                    this.Overlay_change = true;
                    OnPropertyChanged("Sum");
                }
            }
            catch (Exception ex)
            {

                appvm.LogException(ex);
            }
           
        }
       

        private double CalculateTotal(ObservableCollection<Product> lijst)
        {
            if (lijst != null)
            {
                double totaal = 0;
                for (int i = 0; i < lijst.Count; i++)
                {
                    totaal += lijst[i].Amount * lijst[i].Price;

                }
                return totaal;
            }

            return 0;
        }
        private double CalculateTotal(Product product)
        {
            double totaal = 0;
           
            totaal += product.Amount * product.Price;

            return totaal;
        }

        private void AddAmount(Product product)
        {
            try
            {
                if (this.SelectedCustomer == null || this.SelectedCustomer.ID == 0)
                {
                    throw new Exception("Geen gebruiker geselecteerd");
                }

                ObservableCollection<Product> lijst = this.Basket;
                if (lijst == null) lijst = new ObservableCollection<Product>();
                int id = GetIdIn(lijst, product);//kijk of product al in lijst staat

                //product bestaat nog niet inde lijst
                if (id == -1)
                {
                    lijst.Add(product);
                    id = lijst.Count - 1;
                }

                if (IsLimietOk(lijst, lijst[id]))
                {
                    lijst[id].Amount++;

                }
                else
                {
                    throw new Exception("De gebruiker is over het limiet");
                }


                if (this.Basket != null) this.Basket = null; //eerst null maken anders werkt de trigger niet
                this.Basket = lijst;

            }
            catch (Exception ex)
            {
                appvm.LogException(ex);
                return;
            }




        }
        private void RemoveAmount(Product product)
        {
            try
            {
                ObservableCollection<Product> lijst = this.Basket;
                if (lijst == null) lijst = new ObservableCollection<Product>();
                int id = GetIdIn(lijst, product);

                if (id == -1) lijst.Add(product);

                if (lijst[id].Amount > 1)
                {
                    lijst[id].Amount--;
                }
                else
                {
                    lijst[id].Amount = 0;
                    lijst.Remove(product);

                }


                if (this.Basket != null) this.Basket = null; //eerst null maken anders werkt de trigger niet
                this.Basket = lijst;
            }
            catch (Exception ex)
            {
                appvm.LogException(ex);
                return;
            }
            
        }
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






        private ObservableCollection<Product> Sort(ObservableCollection<Product> basket)
        {
            if (basket != null)
            {
                List<Product> lijst = basket.OrderBy(r => r.Name).ToList<Product>();
                ObservableCollection<Product> o = new ObservableCollection<Product>(lijst);
                return o;
            }
            return null;
        }


        #region navigate and save
       

        //buttons
        private async void Pay()
        {
            if (this.Basket != null)
            {
                bool canSave = false;
                for (int i = 0; i < this.Basket.Count; i++)
                {
                    string ex = this.SelectedCustomer.Name + " heeft " + CalculateTotal(this.Basket[i]).ToString() + " betaald door " + this.SelectedEmployee.Name.ToString();
                    appvm.LogException(new Exception(ex));
                   

                    using (HttpClient client = new HttpClient())
                    {
                        Sale sale = new Sale()
                        {
                            CustomerID = this.SelectedCustomer.ID,
                            RegisterID = this.SelectedRegister.ID,
                            ProductID = this.Basket[i].ID,
                            Amount = this.Basket[i].Amount,
                            TotalPrice = CalculateTotal(this.Basket[i])
                        };
                        client.SetBearerToken(ApplicationVM.Token.AccessToken);
                        string json = JsonConvert.SerializeObject(sale);
                        HttpResponseMessage response = await client.PostAsync(URL_sales, new StringContent(json, Encoding.UTF8, "application/json"));
                        if (response.IsSuccessStatusCode)
                        {

                            canSave = true;

                        }
                        else
                        {
                            canSave = false;
                        }

                    }
                }
                if (canSave == true)
                {
                    reset();
                    
                    
                }
            }
           
            
        }

        private void reset()
        {
            this.SelectedCustomer = null;
            this.Customers = null;
            this.Products = null;
            this.Basket = null;
            this.EID_visible = true;
            //
            this.Customers_visible = true;
            //overlays hidden
            this.Overlay_pay = false;
            this.Overlay_change = false;

            GetData();
        }
        private void New()
        {
            //alle controls leegmaken (herladen)
            string ex = this.SelectedEmployee.Name + " heeft een nieuwe klant";
            appvm.LogException(new Exception(ex));
            reset();
            
        }
        private void Cancel()
        {

            string ex = this.SelectedEmployee.Name + " heeft een nieuwe klant geanuleerd";
            appvm.LogException(new Exception(ex));

            this.Overlay_pay = false;
            OnPropertyChanged("Overlay_pay");
        }
       
        private void EndSession()
        {
            //einde toevoegen
            appvm.EndSession();

            appvm.ChangePage(new PageStartVM());
        }
        //balance
        private async void SaveBalance()
        {
            try
            {
                double balance = Convert.ToDouble(this.SelectedCustomer_New.Balance);
                this.SelectedCustomer.Balance = balance;

                string ex = this.SelectedCustomer_New.Name + " heeft een nieuw saldo van " + this.SelectedCustomer.Balance.ToString() + " euro ";
                appvm.LogException(new Exception(ex));

                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    string json = JsonConvert.SerializeObject(this.SelectedCustomer);
                    HttpResponseMessage response = await client.PutAsync(URL_customers, new StringContent(json, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {

                       
                       

                        Customer selected = this.SelectedCustomer;
                        //GetCustomers();
                        this.SelectedCustomer = null;
                        this.Overlay_change = false;
                        this.SelectedCustomer = selected;
                        OnPropertyChanged("SelectedCustomer");

                       
                    }
                    else
                    {
                        throw new Exception("Klant saldo mislukt");
                    }
                }
            }
            catch (Exception ex)
            {
                appvm.LogException(ex);
                return;
            }
            
        }
        private void CancelBalance()
        {
            this.Overlay_change = false;
            OnPropertyChanged("Overlay_change");
        }

        private void Total()
        {

            this.Overlay_pay = true;
        }

       

        #endregion

        #region data
        private bool GetData()
        {
            var products=GetProducts();
            var customers=GetCustomers();
            var registers=GetRegisters();

            products.Wait();
            customers.Wait();
            registers.Wait();
            if (products.IsCompleted && customers.IsCompleted && registers.IsCompleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<bool> GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL));

                //do wait until its done
                task.Wait();
                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));
                HttpResponseMessage response = await task;
                //einde wait

                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                   
                    this.Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                    if (this.Products.Count==0)
                    {
                        this.Products.Add(new Product()
                        {
                            Name="Geen producten",
                            ID=0
                        });
                    }
                    return true;
                }
                else
                {
                    appvm.LogException(new Exception("Producten ophalen mislukt"));
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

                //do wait until its done
                task.Wait();
                HttpResponseMessage response = await task;
                //einde wait

                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                  
                    List<Customer> lijst = JsonConvert.DeserializeObject<List<Customer>>(json).OrderBy(r => r.Name).ToList<Customer>();
                    
                    this.Customers = new ObservableCollection<Customer>(lijst);

                    
                    return true;
                }
                else
                {
                    appvm.LogException(new Exception("Gebruiker ophalen mislukt"));
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

                //do wait until its done
                task.Wait();
                HttpResponseMessage response = await task;
                //einde wait


                
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    this.Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                    return true;

                }
                else
                {
                    appvm.LogException(new Exception("Kassa ophalen mislukt"));
                }
            }
            return false;


        }

        private void LoadEID()
        {
            this.EID = EIDReader.LaadEID();
            if (this.EID == null) appvm.LogException(new Exception("EID lezen mislukt"));

            Customer customer = EIDReader.ConvertTo(typeof(Customer), this.EID) as Customer;

            if (customer != null)
            {
                List<Customer> lijst = this.Customers.Where(r => r.RegisterNumber == customer.RegisterNumber).ToList<Customer>();
                if (lijst.Count!=0 && lijst!=null)
                {
                    this.SelectedCustomer = null;
                    this.SelectedCustomer = lijst[0];
                    
                }
                
            }
            else
            {
                string ex = "Laden eid mislukt";
                appvm.LogException(new Exception(ex));

                this.Customers_visible = true;
                this.EID_visible = false;
            }



        }

        #endregion



        #endregion
    }
}
