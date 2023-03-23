using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TxtInterfaces.Classes;
using TxtInterfaces.Data;
using TxtInterfaces.Framework;



namespace TxtInterfaces.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private AsyncCommand<object, bool> _loadMe;
        private ICommand _closeApp;
        private ICommand _loadChild;
        private string _caption = "TxtInterFace";
        private bool _enabled = true;
        private Cursor _curs;
        public ICommand CloseApp
        {
            get
            {
                if (_closeApp == null) _closeApp = new RelayCommand<object>(CloseAppExecute);
                return _closeApp;
            }
        }
        private void CloseAppExecute(object parameter)
        {
            Application.Current.MainWindow.Close();
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (value == _currentView) return;
                _currentView = value;
                if (PropertyChanged != null)
                    PropertyChanged.Raise(this, o => o.CurrentView);
            }
        }

        
        public ICommand LoadChild
        {
            get
            {
                if (_loadChild == null) _loadChild = new RelayCommand<object>(LoadChildExecute);
                return _loadChild;
            }
        }
        private void LoadChildExecute(object parameter)
        {
            string p = parameter.ToString();
            switch (parameter.ToString())
            {
               case "Seasons": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.SeasonsViewModel") CurrentView = new SeasonsViewModel(); Caption="Seasons";return;
               case "Countries": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.CountriesViewModel") CurrentView = new CountriesViewModel(); Caption = "Countries"; return;
               case "PlantPrio": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.PlantsViewModel") CurrentView = new PlantsViewModel(); Caption = "Plants"; return;
               case "Customers": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.CustomersViewModel") CurrentView = new CustomersViewModel(); Caption = "Customers"; return;
               case "Crosscustomers": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.XCustomersViewModel") CurrentView = new XCustomersViewModel(); Caption = "Cross Customers"; return;
               case "SAPvsTxt": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.SAPvsTxtViewModel") CurrentView = new SAPvsTxtViewModel(); Caption = "SAP vs TXT";  return;
               case "ImpCust": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.ImpCustViewModel") CurrentView = new ImpCustViewModel(); Caption = "Import Customers"; return;
               case "ImpSeason": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.ImpSeasonsViewModel") CurrentView = new ImpSeasonsViewModel(); Caption="Import Seasons";return;
               case "SizeSplit": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.SizeSplitViewModel") CurrentView = new SizeSplitViewModel(); Caption = "Size Split"; return;
               case "EOPS": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.CpyEopsViewModel") CurrentView = new CpyEopsViewModel(); Caption = "Copy EOS"; return;
               case "SplitChannel": if (CurrentView == null || CurrentView.GetType().ToString() != "TxtInterfaces.ViewModel.SplitChannelViewModel") CurrentView = new SplitChannelViewModel(); Caption = "Split Channel"; return;
            }
        }
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
                if (PropertyChanged != null)
                    PropertyChanged.Raise(this, o => o.Enabled);
            }
        }
        public string Caption
        {
            get { return _caption; }
            set { _caption = value;
                if (PropertyChanged != null)
                    PropertyChanged.Raise(this, o => o.Caption);
            }
        }
        public string Content
        { set { LoadChildExecute(value); } }
        public Cursor Curs
        {
            get
            {
                return _curs;
            }
            set
            {
                if (value == _curs)
                    return;
                _curs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Curs"));
            }
        }
        public AsyncCommand<object, bool> LoadMe
        {
            get
            {
                if (_loadMe == null)
                    _loadMe = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadParams();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !LoadMe.IsWorking; });
                return _loadMe;
            }
        }
        private bool LoadParams()
        {
            General.ChangeActivity(false);
            ClsData _clsData = new ClsData();
            General.parameters = _clsData.GetParams();
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
   
}
