using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
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
    public class PageProductenVM_add: ObservableObject,IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public PageProductenVM_add()
        {
            this.Product = new Product();
            appvm.SelectPage("Producten");
        }

        #region props
        public string Name
        {
            get { return "Producten > add"; }
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

        private Product _Product;

        public Product Product
        {
            get { return _Product; }
            set { _Product = value; OnPropertyChanged("Product"); }
        }

       

        #endregion

        #region commands
        public ICommand Insert
        {
            get { return new RelayCommand(InsertProduct,Product.IsValid); }

        }
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelProduct); }

        }

        //image commands    
       
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
        private void CancelProduct()
        {
            appvm.ChangePage(new PageProductenVM());
        }
        private async void InsertProduct()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(this.Product);
                HttpResponseMessage response = await client.PostAsync(PageProductenVM.URL, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    appvm.ChangePage(new PageProductenVM());
                }
            }
        }


        private void AddImage()
        {
            Product product = this.Product;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {

                product.Picture = GetPhoto(ofd.FileName);
                this.Product = null;
                this.Product = product;
            }
        }
        private void AddImage(DragEventArgs s)
        {
            string[] location = (string[])s.Data.GetData(DataFormats.FileDrop);
            Product product = this.Product;
            product.Picture = GetPhoto(location[0].ToString());
            this.Product = null;
            this.Product = product;

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
