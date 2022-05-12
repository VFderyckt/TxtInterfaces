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
    class CountriesViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetCountriesResult> _lstCountries;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<string> _lstFather;
        private List<int> _lstLevel;
        private AsyncCommand<object, bool> _getCountries;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetCountriesResult _country;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addCountry;
        private ICommand _duplicate;
        public sp_Interface_GetCountriesResult Country
        {
            get { return _country; }
            set
            {
                _country = value;
                PropertyChanged.Raise(this, o => o.Country);
            }
        }
        public AsyncCommand<object, bool> GetCountries
        {
            get
            {
                if (_getCountries == null)
                    _getCountries = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadCountries();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetCountries.IsWorking; });
                return _getCountries;
            }
        }

        public List<sp_Interface_GetCountriesResult> LstCountries
        {
            get { return _lstCountries; }
            set
            {
                _lstCountries = value;
                PropertyChanged.Raise(this, o => o.LstCountries);
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
        private bool LoadCountries()
        {
            General.ChangeActivity(false);
            LstCountries = _clsData.GetCountries();
            LstModel = _clsData.GetFcstModels();
            LstLevel = new List<int> { 0, 1 };
            SelChangeExecute(LstCountries.First());
            return true;
        }

        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetCountriesResult p = ((sp_Interface_GetCountriesResult)Parameter);
            if (p != null)
            {
                Country = new sp_Interface_GetCountriesResult()
                {
                    Fcst_model = p.Fcst_model,
                    CountryId = p.CountryId,
                    CountryDesc = p.CountryDesc,
                    Country_FatherId=p.Country_FatherId,
                    Country_Level=p.Country_Level,
                    CrossCountryId=p.CrossCountryId,
                    Currency_code=p.Currency_code,
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
            }
            else
            {
                _country = new sp_Interface_GetCountriesResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                sp_Interface_GetFcstModelsResult x = (sp_Interface_GetFcstModelsResult)Parameter;
                LstFather = LstCountries.Where(a => a.Fcst_model == x.fcst_model && (a.Country_Level == 1 || a.CountryId == "")).Select(a => a.CountryId).Union(
                    LstCountries.Where(a => a.Fcst_model == x.fcst_model).Select(a => a.Country_FatherId)).Distinct().ToList();
            }
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
        public ICommand AddCountry
        {
            get
            {
                if (_addCountry == null) _addCountry = new RelayCommand<object>(AddCountryExecute);
                return _addCountry;
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
            sp_Interface_GetCountriesResult xCtry = Country;
            xCtry.Log_upd_user = "";
            SelChangeExecute(LstCountries.First());
            Country = xCtry;
        }
        private void AddCountryExecute(object Parameter)
        {
            if(Country.Log_upd_user=="" && LstCountries.Any(a=>a.CountryId==Country.CountryId && a.Fcst_model==Country.Fcst_model))
                MessageBox.Show("This Country already exists in the setup", "Add Country", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Country.CountryId == "" || Country.CrossCountryId == "")
                {
                    MessageBox.Show("Please assign a Country and a CrossCountry", "Add Country", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _clsData.UpdCountries(Country);
                    _getCountries.Execute(this);
                    MessageBox.Show("Your country has been saved.", "Country Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelChangeExecute(LstCountries.First());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
