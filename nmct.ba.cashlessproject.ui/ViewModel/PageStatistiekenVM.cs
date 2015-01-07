using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.helper;
using nmct.ba.cashlessproject.model;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class PageStatistiekenVM : ObservableObject, IPage
    {

        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/sale";
        public BackgroundWorker bw = new BackgroundWorker();

        public PageStatistiekenVM()
        {
            if (ApplicationVM.Token != null)
            {

                this.StartDate = DateTime.Now.AddMonths(-2);
                this.EndDate = DateTime.Now.AddHours(2);

                var task = GetSales();
                //task.Wait();

                //if (task.IsCompleted)
                //{
                //    GetGraphPoints();
                //}

                //visible
               

                this.Stats = new List<string>();
                this.Stats.Add("Alles");
                this.Stats.Add("Bij product");
                this.Stats.Add("Bij kassa");

                this.SelectedID = this.Stats[0];
            }
        }

        

        #region props
        public string Name
        {
            get { return "Sale"; }
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
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/stat.png", UriKind.Relative)); }
        }

        private ObservableCollection<Sale> _Sales;

        public ObservableCollection<Sale> Sales
        {
            get { return _Sales; }
            set { _Sales = value; OnPropertyChanged("Sales"); OnPropertyChanged("Sales_filter"); }
        }
        public ObservableCollection<Sale> Sales_filter
        {
            get { return FilterByDate(StartDate,EndDate); }
        }

        //custom lijsten
        #region custom lists
        private ObservableCollection<Sale.ByProduct> _Sales_product;

        public ObservableCollection<Sale.ByProduct> Sales_product
        {
            get { return _Sales_product; }
            set { _Sales_product = value; OnPropertyChanged("Sales_product"); OnPropertyChanged("Sales_product_filter"); }
        }
        public ObservableCollection<Sale.ByProduct> Sales_product_filter
        {
            get { return FilterByDate_product(StartDate, EndDate); }
        }

        private ObservableCollection<Sale.ByRegister> _Sales_register;

        public ObservableCollection<Sale.ByRegister> Sales_register
        {
            get { return _Sales_register; }
            set { _Sales_register = value; OnPropertyChanged("Sales_register"); OnPropertyChanged("Sales_register_filter"); }
        }
        public ObservableCollection<Sale.ByRegister> Sales_register_filter
        {
            get { return FilterByDate_register(StartDate, EndDate); }
        }

        #endregion
        private ObservableCollection<Chart> _Graph;

        public ObservableCollection<Chart> Graph
        {
            get { return _Graph; }
            set { _Graph = value; OnPropertyChanged("Graph"); }
        }
   

        private BitmapImage _LoadingImage;

        public BitmapImage LoadingImage
        {
            get { return _LoadingImage; }
            set { _LoadingImage = value; OnPropertyChanged("LoadingImage"); }
        }
        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { _StartDate = value; OnPropertyChanged("StartDate");
            OnPropertyChanged("Sales_filter"); OnPropertyChanged("Sales_product_filter"); OnPropertyChanged("Sales_register_filter");
            }
        }

        private DateTime _EndDate;

        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {   _EndDate = value; OnPropertyChanged("EndDate");
                OnPropertyChanged("Sales_filter"); OnPropertyChanged("Sales_product_filter"); OnPropertyChanged("Sales_register_filter");

            }
        }
        #region visible
        private bool _Full;

        public bool Full
        {
            get { return _Full; }
            set { _Full = value; OnPropertyChanged("Full"); }
        }
        private bool _ByProduct;

        public bool ByProduct
        {
            get { return _ByProduct; }
            set { _ByProduct = value; OnPropertyChanged("ByProduct"); }
        }

        private bool _ByRegister;

        public bool ByRegister
        {
            get { return _ByRegister; }
            set { _ByRegister = value; OnPropertyChanged("ByRegister"); }
        }
        private string _SelectedID;

        public string SelectedID
        {
            get { return _SelectedID; }
            set
            {
                _SelectedID = value;
                if (_SelectedID!=null)
                {
                    if (this.Stats.FindIndex(x => x.Contains(_SelectedID)) >= 0)
                    {
                        int i = this.Stats.FindIndex(x => x.Contains(_SelectedID));
                        switch (i)
                        {
                            case 0: this.Full = true; this.ByProduct = false; this.ByRegister = false;
                                OnPropertyChanged("Full"); OnPropertyChanged("ByRegister"); OnPropertyChanged("ByProduct"); break;
                            case 1: this.ByProduct = true; this.Full = false; this.ByRegister = false;
                                OnPropertyChanged("ByProduct"); OnPropertyChanged("ByRegister"); OnPropertyChanged("Full"); break;
                            case 2: this.ByRegister = true; this.Full = false; this.ByProduct = false;
                                OnPropertyChanged("ByRegister"); OnPropertyChanged("ByProduct"); OnPropertyChanged("Full"); break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    _SelectedID = this.Stats[0];
                }
                
                
                OnPropertyChanged("SelectedID");

            }
        }
        private List<string> _Stats;

        public List<string> Stats
        {
            get { return _Stats; }
            set { _Stats = value; OnPropertyChanged("Stats"); }
        }
        

        #endregion


        #endregion

        #region commands
        public ICommand ExportCommand
        {
            get { return new RelayCommand(Export); }

        }

       
     
        #endregion

        #region functions

        private async Task<ObservableCollection<Sale>> GetSales()
        {
            List<Sale> lijst = new List<Sale>();

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
                    lijst = JsonConvert.DeserializeObject<List<Sale>>(json);

                    lijst = lijst.OrderBy(r => r.Timestamp).ToList<Sale>();
                    ObservableCollection<Sale> sortedLijst = new ObservableCollection<Sale>(lijst);


                    this.StartDate = TimeConverter.ToDateTime(sortedLijst[0].Timestamp);
                    this.Sales = sortedLijst;
                    this.Sales_product = GetByProduct();
                    this.Sales_register = GetByRegister();


                    if (this.Sales.Count == 0)
                    {
                        this.Error = "Geen statistieken beschikbaar";
                    }

                    return sortedLijst;

                }
            }

            return null;
        }

        private ObservableCollection<Sale.ByRegister> GetByRegister()
        {
            List<Sale> sales = this.Sales.ToList();
            List<Sale.ByRegister> export = new List<Sale.ByRegister>();

            var result = from s in sales
                         group s by s.ProductID into g
                         select new Sale.ByRegister
                         {
                             RegisterID = g.First().RegisterID,
                             RegisterName = g.First().RegisterName,
                             RegisterDevice = g.First().RegisterDevice,
                             Timestamp=g.First().Timestamp,
                             Sales = g.ToList<Sale>()
                         };
            export = result.ToList<Sale.ByRegister>();


            return new ObservableCollection<Sale.ByRegister>(export);
        }

        private ObservableCollection<Sale.ByProduct> GetByProduct()
        {
            List<Sale> sales = this.Sales.ToList();
            List<Sale.ByProduct> export = new List<Sale.ByProduct>();

            var result = from s in sales
                          group s by s.ProductID into g
                          select new Sale.ByProduct
                          {
                              ProductID=g.First().ProductID,
                              ProductName=g.First().ProductName,
                              ProductPrice= g.First().ProductPrice,
                              ProductPicture= g.First().ProductPicture,
                              ProductDescription=g.First().ProductDescription,
                              Timestamp=g.First().Timestamp,
                              Sales=g.ToList<Sale>()
                          };
            export = result.ToList<Sale.ByProduct>();


            return new ObservableCollection<Sale.ByProduct>(export);
        }


        private ObservableCollection<Sale> FilterByDate(DateTime SelectedStartDate, DateTime SelectedEndDate)
        {
            if (SelectedStartDate != null && SelectedEndDate!=null && this.Sales!=null)
            {

              
                    List<Sale> lijst = this.Sales.Where(r => TimeConverter.ToDateTime(r.Timestamp).Date >= SelectedStartDate.Date && TimeConverter.ToDateTime(r.Timestamp).Date < SelectedEndDate.Date).ToList<Sale>();
                    ObservableCollection<Sale> o = new ObservableCollection<Sale>(lijst);
                    return o;
                
            }
            else
            {
                return this.Sales;
            }
        }
        private ObservableCollection<Sale.ByProduct> FilterByDate_product(DateTime SelectedStartDate, DateTime SelectedEndDate)
        {
            if (SelectedStartDate != null && SelectedEndDate != null && this.Sales != null)
            {


                    List<Sale.ByProduct> lijst = this.Sales_product.Where(r => TimeConverter.ToDateTime(r.Timestamp).Date >= SelectedStartDate.Date && TimeConverter.ToDateTime(r.Timestamp).Date < SelectedEndDate.Date).ToList<Sale.ByProduct>();
                    ObservableCollection<Sale.ByProduct> o = new ObservableCollection<Sale.ByProduct>(lijst);
                    return o;
               

            }
            else
            {
                return this.Sales_product;
            }
        }
        private ObservableCollection<Sale.ByRegister> FilterByDate_register(DateTime SelectedStartDate, DateTime SelectedEndDate)
        {
            if (SelectedStartDate != null && SelectedEndDate != null && this.Sales != null)
            {

              
                    List<Sale.ByRegister> lijst = this.Sales_register.Where(r => TimeConverter.ToDateTime(r.Timestamp).Date >= SelectedStartDate.Date && TimeConverter.ToDateTime(r.Timestamp).Date < SelectedEndDate.Date).ToList<Sale.ByRegister>();
                    ObservableCollection<Sale.ByRegister> o = new ObservableCollection<Sale.ByRegister>(lijst);
                    return o;
            }
            else
            {
                return this.Sales_register;
            }
        }

        private void GetGraphPoints()
        {
            
                this.Graph = new ObservableCollection<Chart>();
                List<Chart> result = this.Sales.GroupBy(r => r.ProductID).Select(r => new Chart
                {
                    X = r.First().ProductID,
                    Y = r.Sum(c => c.Amount)
                }).ToList();
                foreach (Chart item in result)
                {
                    this.Graph.Add(item);
                }
          

           
        }


        private void Export()
        {
            bw.DoWork+=bw_DoWork;
            bw.RunWorkerCompleted+=bw_RunWorkerCompleted;
            if (bw.IsBusy != true)
            {
                bw.RunWorkerAsync();
            } 
        }

        #region do work fileDownload
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = "";

            sf.Filter = "Excel (*.xlsx) |*.xlsx;*";
        
            if (sf.ShowDialog() == true)
            {
                // Now here's our save folder
                string savePath = Path.GetDirectoryName(sf.FileName);
                ExportExcel.SaveFile(sf.FileName,this.Sales.ToList());
            }

        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("File succesvol geexporteerd!", "Gelukt");
        }
        #endregion

        #endregion
       
    }
}
