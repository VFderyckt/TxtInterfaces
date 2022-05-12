using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TxtInterfaces.Framework;
using TxtInterfaces.Data;
using TxtInterfaces.Classes;
using System.Windows.Input;
using System.Windows;


namespace TxtInterfaces.ViewModel
{
    class CustomersViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetCustomersResult> _lstCustomers;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<string> _lstFather;
        private List<string> _lstType;
        private List<int> _lstLevel;
        private AsyncCommand<object, bool> _getCustomers;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetCustomersResult _Customer;
        private ICommand _selChange;
        private ICommand _selChangeLevel;
        private ICommand _selChangeModel;
        private ICommand _addCustomer;
        private ICommand _duplicate;
        public sp_Interface_GetCustomersResult Customer
        {
            get { return _Customer; }
            set
            {
                _Customer = value;
                PropertyChanged.Raise(this, o => o.Customer);
            }
        }
        public AsyncCommand<object, bool> GetCustomers
        {
            get
            {
                if (_getCustomers == null)
                    _getCustomers = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadCustomers();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetCustomers.IsWorking; });
                return _getCustomers;
            }
        }

        public List<sp_Interface_GetCustomersResult> LstCustomers
        {
            get { return _lstCustomers; }
            set
            {
                _lstCustomers = value;
                PropertyChanged.Raise(this, o => o.LstCustomers);
            }
        }
        public List<int> LstLevel
        {
            get { return _lstLevel; }
            set
            {
                _lstLevel = value;
                PropertyChanged.Raise(this, o => o.LstLevel);
            }
        }
        public List<string> LstType
        {
            get { return _lstType; }
            set
            {
                _lstType = value;
                PropertyChanged.Raise(this, o => o.LstType);
            }
        }
        public List<string> LstFather
        {
            get { return _lstFather; }
            set
            {
                _lstFather = value;
                PropertyChanged.Raise(this, o => o.LstFather);
            }
        }
        public List<sp_Interface_GetFcstModelsResult> LstModel
        {
            get { return _lstModel; }
            set
            {
                _lstModel = value;
                PropertyChanged.Raise(this, o => o.LstModel);
            }

        }
        private bool LoadCustomers()
        {
            General.ChangeActivity(false);
            LstCustomers = _clsData.GetCustomers();
            LstModel = _clsData.GetFcstModels();
            LstLevel = new List<int> { 0, 1, 2 };
            LstType = new List<string>();
            string[] ct = Properties.Settings.Default.CustTypes.Split('|');
            for (int i = 0; i < ct.Length; i++)
            {
                LstType.Add(ct[i]);
            }
            SelChangeExecute(LstCustomers.First());
            return true;
        }

        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetCustomersResult p = ((sp_Interface_GetCustomersResult)Parameter);
            if (p != null)
            {
                Customer = new sp_Interface_GetCustomersResult()
                {
                    Fcst_model = p.Fcst_model,
                    Fcst_Customer=p.Fcst_Customer,
                    Fcst_CustomerD=p.Fcst_CustomerD,
                    Fcst_Cust_Level=p.Fcst_Cust_Level,
                    Fcst_Cust_Father=p.Fcst_Cust_Father,
                    Fcst_Cust_Type=p.Fcst_Cust_Type,
                    Fcst_Excluded=p.Fcst_Excluded,                    
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
            }
            else
            {
                _Customer = new sp_Interface_GetCustomersResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter!=null)
                FillFather(((sp_Interface_GetFcstModelsResult)Parameter).fcst_model,Customer.Fcst_Cust_Level);
        }
        private void SelChangeLevelExecute(object Parameter)
        {
            if (Parameter!=null)
                FillFather(Customer.Fcst_model,Convert.ToInt16(Parameter));
        }

        private void FillFather(string mod,int lvl)
        {
            LstFather = LstCustomers.Where(a=> a.Fcst_model == mod && a.Fcst_Cust_Level == lvl + 1).Select (a=>a.Fcst_Customer).ToList();
        }
        public ICommand SelChange
        {
            get
            {
                if (_selChange == null) _selChange = new RelayCommand<object>(SelChangeExecute);
                return _selChange;
            }
        }
        public ICommand SelChangeModel
        {
            get
            {
                if (_selChangeModel == null) _selChangeModel = new RelayCommand<object>(SelChangeModelExecute);
                return _selChangeModel;
            }
        }
        public ICommand SelChangeLevel
        {
            get
            {
                if (_selChangeLevel == null) _selChangeLevel = new RelayCommand<object>(SelChangeLevelExecute);
                return _selChangeLevel;
            }
        }
        public ICommand AddCustomer
        {
            get
            {
                if (_addCustomer == null) _addCustomer = new RelayCommand<object>(AddCustomerExecute);
                return _addCustomer;
            }
        }
        public ICommand Duplicate
        {
            get
            {
                if (_duplicate == null) _duplicate = new RelayCommand<object>(DuplicateExecute);
                return _duplicate;
            }
        }
        private void DuplicateExecute(object Parameter)
        {
            sp_Interface_GetCustomersResult xCus = Customer;
            xCus.Log_upd_user = "";
            SelChangeExecute(LstCustomers.First());
            Customer = xCus;
        }
        private void AddCustomerExecute(object Parameter)
        {
            if (Customer.Log_upd_user=="" && LstCustomers.Any(a => a.Fcst_Customer == Customer.Fcst_Customer && a.Fcst_model == Customer.Fcst_model))
                MessageBox.Show("This customer is already setup", "Add Customer", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Customer.Fcst_CustomerD == "")
                {
                    MessageBox.Show("Please assign a description", "Add Customer", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _clsData.UpdCustomers(Customer);
                    _getCustomers.Execute(this);
                    MessageBox.Show("Your customer has been saved.", "Customer Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelChangeExecute(LstCustomers.First());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
