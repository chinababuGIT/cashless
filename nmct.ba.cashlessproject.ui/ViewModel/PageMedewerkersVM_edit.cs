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
    public class PageMedewerkersVM_edit: ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

       
        public PageMedewerkersVM_edit()
        {
            GetEmployee();
            appvm.SelectPage("Medewerkers");
        }

        #region props
        public string Name
        {
            get { return "Medewerkers > Edit"; }
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
        #endregion

        #region commands

        public ICommand Update
        {
            get { return new RelayCommand(UpdateEmployee, Employee.IsValid); }
            
        }
        public ICommand Cancel
        {
            get { return new RelayCommand(CancelEmployee); }

        }
        public ICommand Delte
        {
            get { return new RelayCommand(DeleteEmployee); }

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

        private void GetEmployee()
        {
            Employee Employee = appvm.Current as Employee;
            this.Employee = Employee;
        }
        private void CancelEmployee()
        {
            appvm.ChangePage(new PageMedewerkersVM());
        }

        private async void UpdateEmployee()
        {
            using (HttpClient client= new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                string json = JsonConvert.SerializeObject(this.Employee);
                HttpResponseMessage response= await client.PutAsync(PageMedewerkersVM.URL, new StringContent(json,Encoding.UTF8,"application/json"));
                if (response.IsSuccessStatusCode)
	            {
                    appvm.Current = null;
		            appvm.ChangePage(new PageMedewerkersVM());
	            }

            } 
        }
        private async void DeleteEmployee()
        {
            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Weet je zeker dat je " + this.Employee.Name + " wilt verwijderen?", this.Employee.Name + " " + this.Employee.SurName, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.MessageBoxDefaultButton.Button2);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync(PageMedewerkersVM.URL + "?id=" + this.Employee.ID.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        appvm.ChangePage(new PageMedewerkersVM());
                    }
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


        #endregion

    }
}
