using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.Helper;
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
    public class PageKlantenVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/customer";
        public PageKlantenVM()
        {
            if (ApplicationVM.Token != null)
            {
                GetCustomers();
            }
        }

        #region props
        public string Name
        {
            get { return "Klanten"; }
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
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/customer.png", UriKind.Relative)); }
        }

        private ObservableCollection<Customer> _Customers;

        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set { _Customers = value; OnPropertyChanged("Customers"); }
        }

        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }
        private EID _EID;

        public EID EID
        {
            get { return _EID; }
            set { _EID = value; OnPropertyChanged("EID"); }
        }

#endregion

        #region commands
        public ICommand Add
        {
            get { return new RelayCommand(AddCustomer); }
        }
        public ICommand Edit
        {
            get { return new RelayCommand<Customer>(EditCustomer); }
        }
        public ICommand Remove
        {
            get { return new RelayCommand<Customer>(RemoveCustomer); }
        }
        public ICommand EIDCommand
        {
            get { return new RelayCommand(LoadEID); }
        }
        #endregion

        #region functions

        private async void GetCustomers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(new Uri(URL));

                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));

                HttpResponseMessage response = await task;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    this.Customers = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                   
                    if (this.Customers.Count == 0)
                    {
                        this.Error = "Geen klanten beschikbaar";
                    }
                }
            }

        }

        private async Task<Customer> GetCustomer(string registerNumber)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                Task<HttpResponseMessage> task = client.GetAsync(URL + "?registernumber=" + registerNumber.ToString());

                this.LoadingImage = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "../../Themes/images/load.png", UriKind.Absolute));
                task.Wait();
                HttpResponseMessage response = await task;
                this.LoadingImage = null;
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer customer = JsonConvert.DeserializeObject<Customer>(json);
                    return customer;
                }
            }
            return null;
        }
        public void AddCustomer()
        {
            appvm.ChangePage(new PageKlantenVM_add());
        }
        public void EditCustomer(Customer customer)
        {
            appvm.Current = null;
            appvm.Current = customer;
            appvm.ChangePage(new PageKlantenVM_edit());
        }
        public async void RemoveCustomer(Customer customer)
        {
           
            DialogResult result = MessageBox.Show("Weet je zeker dat je " + customer.Name + " wilt verwijderen?", customer.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync(URL + "?id=" + customer.ID.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        this.Customers = null;
                        GetCustomers();
                        appvm.ChangePage(new PageKlantenVM());
                    }
                }
            }
        }

        private void LoadEID()
        {
            this.EID = EIDReader.LaadEID();
            Customer customer = EIDReader.ConvertTo(typeof(Customer), this.EID) as Customer;

            

            if (customer != null)
            {
                Task<Customer> task = GetCustomer(this.EID.RegisterNumber);
                task.Wait();
                if (task.IsCompleted)
                {
                    customer = task.Result;
                    if (customer!=null && customer.ID!=0)
                    {
                        appvm.Current = null;
                        appvm.Current = customer;
                        appvm.ChangePage(new PageKlantenVM_edit());

                    }
                    else
                    {
                        appvm.Current = null;
                        appvm.Current = EIDReader.ConvertTo(typeof(Customer), this.EID) as Customer;
                        appvm.ChangePage(new PageKlantenVM_add());
                    }
                   
                }
                

                
            }
            else
            {
                DialogResult result = MessageBox.Show("De kaartlezer is niet aangesloten", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
           
           
        }

        #endregion

    }
}
