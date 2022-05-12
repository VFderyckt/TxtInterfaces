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
    class XCustomersViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetXCustomersResult> _lstXCustomers;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetXCountriesResult> _lstXCtry;
        private List<sp_Interface_GetXCountriesResult> _lstXCtries;
        private List<sp_Interface_GetCustomersDoorsResult> _lstCustDoor;
        private List<string> _lstCust;
        private List<string> _lstDoor;
        private List<string> _lstXDoor;
        private AsyncCommand<object, bool> _getXCustomers;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetXCustomersResult _XCustomer;
        private sp_Interface_GetXCountriesResult _selXCtry;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _selChangeCust;
        private ICommand _selChangeDoor;
        private ICommand _selChangeXCust;
        private ICommand _addXCustomer;
        private ICommand _duplicate;

        public sp_Interface_GetXCustomersResult XCustomer
        {
            get { return _XCustomer; }
            set
            {
                _XCustomer = value;
                PropertyChanged.Raise(this, o => o.XCustomer);
            }
        }
        public sp_Interface_GetXCountriesResult SelXCtry
        {
            get { return _selXCtry; }
            set { _selXCtry = value;
                PropertyChanged.Raise(this, o => o.SelXCtry);}
        }
        public AsyncCommand<object, bool> GetXCustomers
        {
            get
            {
                if (_getXCustomers == null)
                    _getXCustomers = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadXCustomers();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetXCustomers.IsWorking; });
                return _getXCustomers;
            }
        }

        public List<sp_Interface_GetXCustomersResult> LstXCustomers
        {
            get { return _lstXCustomers; }
            set
            {
                _lstXCustomers = value;
                PropertyChanged.Raise(this, o => o.LstXCustomers);
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
        public List<sp_Interface_GetXCountriesResult> LstXCtry
        {
            get { return _lstXCtry; }
            set
            {
                _lstXCtry = value;
                PropertyChanged.Raise(this, o => o.LstXCtry);
            }

        }
        public List<string> LstCust
        {
            get { return _lstCust; }
            set { _lstCust = value;
                PropertyChanged.Raise(this, o => o.LstCust);
            }
        }
        public List<string> LstDoor
        {
            get { return _lstDoor; }
            set { _lstDoor = value;
                PropertyChanged.Raise(this, o => o.LstDoor);
            }
        }
        public List<string> LstXDoor
        {
            get { return _lstXDoor; }
            set
            {
                _lstXDoor = value;
                PropertyChanged.Raise(this, o => o.LstXDoor);
            }
        }
        private bool LoadXCustomers()
        {
            General.ChangeActivity(false);
            LstXCustomers = _clsData.GetXCustomers();
            LstModel = _clsData.GetFcstModels();
            _lstXCtries = _clsData.GetXCountries();
            _lstCustDoor = _clsData.GetCustomersDoors();
            SelChangeExecute(LstXCustomers.First());
            return true;
        }

        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetXCustomersResult p = ((sp_Interface_GetXCustomersResult)Parameter);
            if (p != null)
            {
                XCustomer = new sp_Interface_GetXCustomersResult()
                {
                    Fcst_model = p.Fcst_model,
                    Customer_code=p.Customer_code,
                    Customer_door=p.Customer_door,
                    CrossCustomer_code=p.CrossCustomer_code,
                    CrossCustomer_door=p.CrossCustomer_door,
                    CrossCustomer_country=p.CrossCustomer_country,
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
                SelXCtry = _lstXCtries.Where(a => a.countryId == XCustomer.CrossCustomer_country).FirstOrDefault();
            }
            else
            {
                _XCustomer = new sp_Interface_GetXCustomersResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                string p = ((sp_Interface_GetFcstModelsResult)Parameter).fcst_model;
                LstXCtry = _lstXCtries.Where(a => a.fcst_model == p).ToList();
                LstCust = _lstCustDoor.Where(a => a.Fcst_Model == p).Select(a => a.Customer_Code).Distinct().ToList();
                LstXDoor = LstDoor = new List<string>();
            }
        }
        private void SelChangeCustExecute(object Parameter)
        {
            if (Parameter != null && XCustomer.Log_upd_user=="")
            {
                _lstDoor = _lstCustDoor.Where(a => a.Fcst_Model == XCustomer.Fcst_model && a.Customer_Code == Parameter.ToString()).Select(a => a.Customer_Door).ToList();
                _lstDoor.Insert(0, "ALL");
                LstDoor = _lstDoor;
            }
            else
            {
                LstDoor = Parameter!=null?new List<string> { XCustomer.Customer_door }:new List<string>();
            }
        }
        private void SelChangeDoorExecute(object Parameter)
        {
            if (Parameter != null && XCustomer.Log_upd_user=="")
                BuildXDoor(XCustomer.Customer_code,Parameter.ToString());
        }
        private void SelChangeXCustExecute(object Parameter)
        {
            if (Parameter != null)
                BuildXDoor(Parameter.ToString(), XCustomer.Customer_door);
        }
        private void BuildXDoor(string CustCode,string CustDoor)
        {
            _lstXDoor = _lstCustDoor.Where(a => a.Fcst_Model == XCustomer.Fcst_model && a.Customer_Code == CustCode).Select(a => a.Customer_Door).ToList();
            if (CustCode==XCustomer.Customer_code && CustDoor=="ALL")
                _lstXDoor.Insert(0, "ALL");
            LstXDoor = _lstXDoor;
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
        public ICommand SelChangeCust
        {
            get
            {
                if (_selChangeCust == null) _selChangeCust = new RelayCommand<object>(SelChangeCustExecute);
                return _selChangeCust;
            }
        }
        public ICommand SelChangeDoor
        {
            get
            {
                if (_selChangeDoor == null) _selChangeDoor = new RelayCommand<object>(SelChangeDoorExecute);
                return _selChangeDoor;
            }
        }
        public ICommand SelChangeXCust
        {
            get
            {
                if (_selChangeXCust == null) _selChangeXCust = new RelayCommand<object>(SelChangeXCustExecute);
                return _selChangeXCust;
            }
        }
        public ICommand AddXCustomer
        {
            get
            {
                if (_addXCustomer == null) _addXCustomer = new RelayCommand<object>(AddXCustomerExecute);
                return _addXCustomer;
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
            sp_Interface_GetXCustomersResult xCus = XCustomer;
            xCus.Log_upd_user = "";
            SelChangeExecute(LstXCustomers.First());
            XCustomer = xCus;
        }
        private void AddXCustomerExecute(object Parameter)
        {
            XCustomer.CrossCustomer_country = SelXCtry.countryId;
            if ((XCustomer.Customer_code == "" || XCustomer.Customer_door == "" ||XCustomer.CrossCustomer_code==""||XCustomer.CrossCustomer_door=="") && Parameter.ToString() != "Del")
                MessageBox.Show("Please assign customer,door,crosscustomer and crossdoor", "Add XCustomer", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (XCustomer.Log_upd_user=="" && LstXCustomers.Any(a => a.Fcst_model == XCustomer.Fcst_model && a.Customer_code == XCustomer.Customer_code && (a.Customer_door == XCustomer.Customer_door || a.Customer_door == "ALL")))
                    MessageBox.Show("There is already a setup for this customer/door in the database","Add XCustomer", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (XCustomer.CrossCustomer_door == "ALL" && XCustomer.Customer_door == "ALL" && XCustomer.CrossCustomer_country == null && Parameter.ToString() != "Del")
                        MessageBox.Show("Please assign CrossCountry Code", "Add XCustomer", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        switch (Parameter.ToString())
                        {
                            case "Del":
                                if (MessageBox.Show("Are you sure you want to delete current row", "Delete XCustomer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                { XCustomer.Log_upd_user = "Del"; goto default; }
                                else
                                    break;
                            default:
                                _clsData.UpdXCustomer(XCustomer);
                                _getXCustomers.Execute(this);
                                MessageBox.Show("Your XCustomer has been " + (XCustomer.Log_upd_user == "Del" ? "Deleted" : "Saved") + ".", "XCustomer Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                                SelChangeExecute(LstXCustomers.First());
                                break;
                        }
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
