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
    class ImpCustViewModel:INotifyPropertyChanged
    {
        private List<sp_Interface_GetImportCustomersResult> _lstImpCust;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetCustomersResult> _lstFcstCust;
        private List<string> _lstFCust;
        private AsyncCommand<object, bool> _getImpCust;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetImportCustomersResult _impCust;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addImpCust;
        private ICommand _duplicate;
        public sp_Interface_GetImportCustomersResult ImpCust
        {
            get { return _impCust; }
            set
            {
                _impCust = value;
                PropertyChanged.Raise(this, o => o.ImpCust);
            }
        }
        public AsyncCommand<object, bool> GetImpCust
        {
            get
            {
                if (_getImpCust == null)
                    _getImpCust = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadImpCust();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetImpCust.IsWorking; });
                return _getImpCust;
            }
        }

       
        public List<sp_Interface_GetImportCustomersResult> LstImpCust
        {
            get { return _lstImpCust; }
            set
            {
                _lstImpCust = value;
                PropertyChanged.Raise(this, o => o.LstImpCust);
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
        public List<string> LstFCust
        {
            get { return _lstFCust; }
            set
            {
                _lstFCust = value;
                PropertyChanged.Raise(this, o => o.LstFCust);
            }
        }
        private bool LoadImpCust()
        {
            General.ChangeActivity(false);
            LstImpCust = _clsData.GetImpCust();
            LstModel = _clsData.GetFcstModels();
            _lstFcstCust = _clsData.GetCustomers();
            SelChangeExecute(LstImpCust.First());
            return true;
        }
        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetImportCustomersResult p = ((sp_Interface_GetImportCustomersResult)Parameter);
            if (p != null)
            {
                ImpCust = new sp_Interface_GetImportCustomersResult()
                {
                    Fcst_Model = p.Fcst_Model,
                    Fcst_Priority = p.Fcst_Priority,
                    Fcst_Customer = p.Fcst_Customer,
                    Customer_code=p.Customer_code,
                    Delivery_addresscode=p.Delivery_addresscode,
                    Collection_From=p.Collection_From,
                    Collection_To=p.Collection_To,
                    Product_Category_From=p.Product_Category_From,
                    Product_Category_To=p.Product_Category_To,
                    Division=p.Division,
                    SalesRegion=p.SalesRegion,
                    CCSalesRegion=p.CCSalesRegion,
                    ClassIDFrom=p.ClassIDFrom,
                    ClassIDto=p.ClassIDto,
                    Definition_level=p.Definition_level,
                    Comment=p.Comment,
                    Log_upd_user = p.Log_upd_user,
                    Deleted_Flag=p.Deleted_Flag
                };
            }
            else
            {
                _impCust = new sp_Interface_GetImportCustomersResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter!=null)
                LstFCust = _lstFcstCust.Where(a => a.Fcst_model == ((sp_Interface_GetFcstModelsResult)Parameter).fcst_model && a.Fcst_Cust_Level==0).Select(a => a.Fcst_Customer).ToList();
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
        public ICommand AddImpCust
        {
            get
            {
                if (_addImpCust == null) _addImpCust = new RelayCommand<object>(AddImpCustExecute);
                return _addImpCust;
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
            sp_Interface_GetImportCustomersResult xIC = ImpCust;
            xIC.Log_upd_user = "";
            SelChangeExecute(LstImpCust.First());
            ImpCust = xIC;
        }
        private void AddImpCustExecute(object Parameter)
        {
            if (ImpCust.Log_upd_user == "" && LstImpCust.Any(a => a.Fcst_Model == ImpCust.Fcst_Model && a.Fcst_Priority == ImpCust.Fcst_Priority))
                MessageBox.Show("Priority already in use", "Add Import Customer", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            { 
                if (ImpCust.Fcst_Priority == 0 || ImpCust.Fcst_Customer=="")
                    MessageBox.Show("Please assign a valid Priority and Fcst_Customer", "Add Import Customer", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    switch (Parameter.ToString())
                    {
                        case "Del":
                            if (MessageBox.Show("Are you sure you want to delete current row", "Delete Import Customer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                ImpCust.Log_upd_user = "Del";goto default;
                            }
                            else 
                                break;
                        default:
                            _clsData.UpdImpCust(ImpCust);
                            _getImpCust.Execute(this);
                            MessageBox.Show("Your ImportCustomer has been " + (ImpCust.Log_upd_user == "Del"?"deleted":"saved") + ".", "Import Customer Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                            SelChangeExecute(LstImpCust.First()); break;
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
