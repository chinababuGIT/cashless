using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
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
    public class PageMedewerkersVM_add: ObservableObject,IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public PageMedewerkersVM_add()
        {
            this.Employee = new Employee();

            if (appvm != null && appvm.Current != null)
            {
                Employee employee = appvm.Current as Employee;
                if (employee != null)
                {
                    this.Employee= null;
                    this.Employee = employee;
                }
            }
            appvm.SelectPage("Medewerkers");
            
        }

        #region props
        public string Name
        {
            get { return "Medewerkers > add"; }
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

        private Employee _Employee;

        public Employee Employee
        {
            get { return _Employee; }
            set { _Employee = value; OnPropertyChanged("Employee"); }
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
            get { return new RelayCommand(InsertEmployee, Employee.IsValid); }

        }
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelEmployee); }
        }
        //EID COMMANDS
        public ICommand EIDCommand
        {
            get { return new RelayCommand(LoadEID); }
        }
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
        private void CancelEmployee()
        {
            appvm.ChangePage(new PageMedewerkersVM());
        }
        private async void InsertEmployee()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(this.Employee);
                HttpResponseMessage response = await client.PostAsync(PageMedewerkersVM.URL, new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    appvm.Current = null;
                    appvm.ChangePage(new PageMedewerkersVM());
                }
            }
        }
        private void AddImage()
        {
            Employee employee = this.Employee;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {

                employee.Picture = GetPhoto(ofd.FileName);
                this.Employee = null;
                this.Employee = employee;
            }
        }
        private void AddImage(DragEventArgs s)
        {
            string[] location = (string[])s.Data.GetData(DataFormats.FileDrop);
            Employee employee = this.Employee;
            employee.Picture = GetPhoto(location[0].ToString());
            this.Employee = null;
            this.Employee = employee;

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
            Employee employee = EIDReader.ConvertTo(typeof(Employee), this.EID) as Employee;

            if (employee != null)
            {
                this.Employee = null;
                this.Employee = employee;
                OnPropertyChanged("Insert");
            }
          

            
        }

        #endregion




       
    }
}
