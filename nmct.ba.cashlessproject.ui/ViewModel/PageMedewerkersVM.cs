using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
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
    
    public class PageMedewerkersVM : ObservableObject, IPage
    {
        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
        public static string URL = "http://localhost:8080/api/employee";
        public PageMedewerkersVM()
        {
             if (ApplicationVM.Token!=null)
            {
            GetEmployees();
                 }
        }

        #region props
        public string Name
        {
            get { return "Medewerkers"; }
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
            get { return new BitmapImage(new Uri("/nmct.ba.cashlessproject.ui;component/Themes/images/employee.png", UriKind.Relative)); }
        }
        private ObservableCollection<Employee> _Employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
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
            get { return new RelayCommand(AddEmployee); }
        }
        public ICommand Edit
        {
            get { return new RelayCommand<Employee>(EditEmployee); }
        }
        public ICommand Remove
        {
            get { return new RelayCommand<Employee>(RemoveEmployee); }
        }
        public ICommand EIDCommand
        {
            get { return new RelayCommand(LoadEID); }
        }
        #endregion

        #region functions

        private async Task<Employee> GetEmployee(string registerNumber)
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
                    Employee employee = JsonConvert.DeserializeObject<Employee>(json);
                    return employee;
                }
            }
            return null;
        }
        private async void GetEmployees()
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
                    this.Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);

                    
                    
                    if (this.Employees.Count == 0)
                    {
                        this.Error = "Geen medewerkers beschikbaar";
                    }
                }
            }

        }
        public void AddEmployee()
        {
            appvm.ChangePage(new PageMedewerkersVM_add());
        }
        public void EditEmployee(Employee employee)
        {
            appvm.Current = employee;
            appvm.ChangePage(new PageMedewerkersVM_edit());
        }
        public async void RemoveEmployee(Employee employees)
        {
            DialogResult result = MessageBox.Show("Weet je zeker dat je " + employees.Name + " wilt verwijderen?", employees.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.Token.AccessToken);
                    HttpResponseMessage response = await client.DeleteAsync(URL + "?id=" + employees.ID.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        this.Employees = null;
                        GetEmployees();
                        appvm.ChangePage(new PageMedewerkersVM());
                    }
                }
            }
        }


        private void LoadEID()
        {
            this.EID = EIDReader.LaadEID();
            Employee employee = EIDReader.ConvertTo(typeof(Employee), this.EID) as Employee;

            if (employee != null)
            {
                Task<Employee> task = GetEmployee(this.EID.RegisterNumber);
                task.Wait();
                if (task.IsCompleted)
                {
                    employee = task.Result;
                    if (employee!=null && employee.ID!=0)
                    {
                        appvm.Current = null;
                        appvm.Current = employee;
                        appvm.ChangePage(new PageMedewerkersVM_edit());
                    }
                    else
                    {
                        appvm.Current = null;
                        appvm.Current = EIDReader.ConvertTo(typeof(Employee), this.EID) as Employee;
                        appvm.ChangePage(new PageMedewerkersVM_add());

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
