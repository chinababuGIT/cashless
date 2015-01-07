using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    public class PageKlantenVM_add:ObservableObject, IPage
    {

        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public PageKlantenVM_add()
        {
            this.Customer = new Customer();

            if (appvm!=null && appvm.Current!=null)
            {
                Customer customer = appvm.Current as Customer;
                if (customer!=null)
                {
                    this.Customer = null;
                    this.Customer = customer;
                }
            }
            
            
            appvm.SelectPage("Klanten");
        }

        #region props
        public string Name
        {
            get { return "Klanten > add"; }
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
        private Customer _Customer;

        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; OnPropertyChanged("Customer"); }
        }

        private EID _EID;

        public EID EID
        {
            get { return _EID; }
            set { _EID = value; OnPropertyChanged("EID"); }
        }

        #endregion

        #region commands
        public ICommand Insert
        {
            get { return new RelayCommand(InsertCustomer, Customer.IsValid); }

        }
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelCustomer); }

        }
        //EID COMMANDS
        public ICommand EIDCommand
        {
            get { return new RelayCommand(LoadEID); }
        }
        //images COMMANDS
        public ICommand AddImageCommand
        {
            get { return new RelayCommand(AddImage); }
        }
        public ICommand DropImageCommand
        {
            get { return new RelayCommand<DragEventArgs>(AddImage); }
        }

        #endregion

        #region functions
        private void CancelCustomer()
        {
            appvm.ChangePage(new PageKlantenVM());
        }
        private async void InsertCustomer()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(this.Customer);
                HttpResponseMessage response = await client.PostAsync(PageKlantenVM.URL, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    appvm.Current = null;
                    appvm.ChangePage(new PageKlantenVM());
                }
            }
        }


        private void AddImage()
        {
            Customer customer = this.Customer;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                
                customer.Picture = GetPhoto(ofd.FileName);
                this.Customer = null;
                this.Customer = customer;
            }
        }
        private void AddImage(DragEventArgs s)
        {
            string[] location = (string[]) s.Data.GetData(DataFormats.FileDrop);
            Customer customer = this.Customer;
            customer.Picture = GetPhoto(location[0].ToString());
            this.Customer = null;
            this.Customer = customer;

        }


        private byte[] GetPhoto(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();

            return data;
        }


        private void LoadEID()
        {
            this.EID = EIDReader.LaadEID();
            Customer customer = EIDReader.ConvertTo(typeof(Customer), this.EID) as Customer;

            if (customer != null)
            {
                this.Customer = null;
                this.Customer = customer;
                OnPropertyChanged("Customer");
                OnPropertyChanged("Insert");
            }
           
        }

  
        #endregion

    }
}
