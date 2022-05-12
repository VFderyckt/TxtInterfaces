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
    class SeasonsViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetSeasonsResult> _lstSeasons;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<string> _lstCollection;
        private List<string> _lstFather;
        private List<int> _lstLevel;
        private AsyncCommand<object, bool> _getSeasons;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetSeasonsResult _season;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addSeason;
        private ICommand _duplicate;
        public sp_Interface_GetSeasonsResult Season
        {
            get { return _season; }
            set
            {
                _season = value;
                PropertyChanged.Raise(this, o => o.Season);
            }
        }
        public AsyncCommand<object, bool> GetSeasons
        {
            get
            {
                if (_getSeasons == null)
                    _getSeasons = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadSeasons();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetSeasons.IsWorking; });
                return _getSeasons;
            }
        }
        
        public List<sp_Interface_GetSeasonsResult> LstSeasons
        {
            get { return _lstSeasons; }
            set
            {
                _lstSeasons = value;
                PropertyChanged.Raise(this, o => o.LstSeasons);
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
        public List<string> LstCollection
        {
            get { return _lstCollection; }
            set
            {
                _lstCollection = value;
                PropertyChanged.Raise(this, o => o.LstCollection);
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
        private bool LoadSeasons()
        {
            General.ChangeActivity(false);
            LstSeasons = _clsData.GetSeasons();
            LstModel = _clsData.GetFcstModels();
            LstLevel = new List<int> { 0, 1 };
            SelChangeExecute(LstSeasons.First());
            return true;
        }
        
        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetSeasonsResult p = ((sp_Interface_GetSeasonsResult)Parameter);
            if (p != null)
            {
                Season = new sp_Interface_GetSeasonsResult() { Fcst_model=p.Fcst_model, Fcst_collection=p.Fcst_collection,Fcst_collection_desc=p.Fcst_collection_desc, Fcst_coll_Level=p.Fcst_coll_Level,
                    Fcst_coll_Father=p.Fcst_coll_Father,Coll_previous=p.Coll_previous,Coll_Previous_Same_Season=p.Coll_Previous_Same_Season,Priority=p.Priority,SMU_flag=p.SMU_flag, Deleted_flag=p.Deleted_flag,
                    Log_upd_user=p.Log_upd_user};
            }
            else
            {
                _season = new sp_Interface_GetSeasonsResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                sp_Interface_GetFcstModelsResult x = (sp_Interface_GetFcstModelsResult)Parameter;
                LstCollection = LstSeasons.Where(a=> a.Fcst_model == x.fcst_model && a.Fcst_coll_Level == 0).Select(a=> a.Fcst_collection).ToList();
                LstFather = LstSeasons.Where(a=> a.Fcst_model == x.fcst_model && (a.Fcst_coll_Level == 1 || a.Fcst_collection == "")).Select(a=> a.Fcst_collection).ToList();
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
        public ICommand AddSeason
        {
            get
            {
                if (_addSeason == null) _addSeason = new RelayCommand<object>(AddSeasonExecute);
                return _addSeason;
            }
        }
        private void DuplicateExecute(object Parameter)
        {
            sp_Interface_GetSeasonsResult xSsn = Season;
            xSsn.Log_upd_user = "";
            SelChangeExecute(LstSeasons.First());
            Season = xSsn;
        }
        private void AddSeasonExecute(object Parameter)
        {
            if (Season.Log_upd_user=="" && LstSeasons.Any(a=>a.Fcst_model==Season.Fcst_model && a.Fcst_collection==Season.Fcst_collection))
                MessageBox.Show("This season is already setup", "Add Season", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Season.Coll_Previous_Same_Season!="" && LstSeasons.Any(a => a.Fcst_model == Season.Fcst_model && a.Coll_Previous_Same_Season == Season.Coll_Previous_Same_Season && a.Fcst_collection!=Season.Fcst_collection))
                    MessageBox.Show("The Previous Collection Same Season is already in use", "Add Season", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (Season.Fcst_collection_desc == "" || Season.Fcst_collection == "")
                    {
                        MessageBox.Show("Please assign a Collection and a Description", "Add Season", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        _clsData.UpdSeason(Season);
                        _getSeasons.Execute(this);
                        MessageBox.Show("Your season has been saved.", "Season Configuration", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelChangeExecute(LstSeasons.First());
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
