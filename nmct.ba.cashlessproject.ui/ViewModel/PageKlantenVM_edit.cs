using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.Kassa;
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
    public class PageKlantenVM_edit:ObservableObject, IPage
    {
         ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

       
        public PageKlantenVM_edit()
        {
            GetCustomer();
            appvm.SelectPage("Klanten");
        }

        #region props
        public string Name
        {
            get { return "Klanten > Edit"; }
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
        #endregion

        #region commands

        public ICommand Update
        {
            get { return new RelayCommand(UpdateCustomer, Customer.IsValid); }
            
        }
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelCustomer); }

        }
        public ICommand Delete
        {
            get { return new RelayCommand(DeleteCustomer); }
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

        private void GetCustomer()
        {
            Customer Customer = appvm.Current as Customer;
            this.Customer = Customer;
        }
        private void CancelCustomer()
        {
            appvm.ChangePage(new PageKlantenVM());
        }

        private async void UpdateCustomer()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                this.Customer.Hidden = false;
                string json = JsonConvert.SerializeObject(this.Customer);
                HttpResponseMessage response= await client.PutAsync(PageKlantenVM.URL, new StringContent(json,Encoding.UTF8,"application/json"));
                if (response.IsSuccessStatusCode)
	            {
                    appvm.Current = null;
		            appvm.ChangePage(new PageKlantenVM());
	            }

            } 
        }
        private async void DeleteCustomer()
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Weet je zeker dat je " + this.Customer.Name + " wilt verwijderen?", this.Customer.Name + " " + this.Customer.SurName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync(PageKlantenVM.URL + "?id=" + this.Customer.ID.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        appvm.ChangePage(new PageKlantenVM());
                    }
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
            string[] location = (string[])s.Data.GetData(DataFormats.FileDrop);
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
       
        #endregion

       
    }
}
