using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using nmct.ba.cashlessproject.ui.kassa.Helper;
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

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class PageKassaVM_klant_add:ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/customer";
        public PageKassaVM_klant_add()
        {
            if (ApplicationVM.Token != null)
            {
                if (appvm != null && appvm.Current != null)
                {

                    EID eid = appvm.Current as EID;

                    this.Customer = new Customer()
                    {
                        Name=eid.FirstName,
                        SurName=eid.LastName,
                        Address=eid.Street,
                        Balance=0,
                        Hidden=false,
                        ID=0,
                        Picture=eid.Image,
                        RegisterNumber=eid.RegisterNumber
                    };
                }
            }
            
        }

        #region props
        public string Name
        {
            get { return "Nieuwe klant"; }
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
       
        

        #endregion

        #region functions
        private void CancelCustomer()
        {
            appvm.ChangePage(new PageStartVM());
        }
        private async void InsertCustomer()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(this.Customer);
                HttpResponseMessage response = await client.PostAsync(URL, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    appvm.Current = this.Customer;
                    appvm.ChangePage(new PageKassaVM_klant());
                }
            }
        }


       


        private byte[] GetPhoto(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();

            return data;
        }



  
        #endregion

    }
}
