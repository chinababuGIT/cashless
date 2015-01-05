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
    public class PageProductenVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/product";

        public PageProductenVM()
        {
            if (ApplicationVM.Token!=null)
            {
                GetProducts();      
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
        private string _Error;

        public string Error
        {
            get { return _Error; }
            set { _Error = value; OnPropertyChanged("Error"); }
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

        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }
       
#endregion  
        
        #region commands

        public ICommand Add
        {
            get { return new RelayCommand(AddProduct); }
        }
        public ICommand Edit
        {
            get { return new RelayCommand<Product>(EditProduct); }
        } 
        public ICommand Remove
        {
            get { return new RelayCommand<Product>(RemoveProduct); }
        }
        #endregion

        #region functions

        private async void GetProducts()
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
                    this.Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                    if (this.Products.Count == 0)
                    {
                        this.Error = "Geen producten beschikbaar";
                    }
                }
            }

        }
        public void AddProduct()
        {
            appvm.ChangePage(new PageProductenVM_add());
        }
        public void EditProduct(Product product)
        {
            appvm.Current = product;
            appvm.ChangePage(new PageProductenVM_edit());
        }
        public async void RemoveProduct(Product product)
        {
            DialogResult result = MessageBox.Show("Weet je zeker dat je "+ product.Name + " wilt verwijderen?" ,product.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync(URL + "?id=" + product.ID.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        this.Products= null;
                        GetProducts();
                        appvm.ChangePage(new PageProductenVM());
                    }
                }
            }
        }  
        #endregion

    }
}
