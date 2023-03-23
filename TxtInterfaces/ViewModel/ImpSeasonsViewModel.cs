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
    class ImpSeasonsViewModel:INotifyPropertyChanged
    {
        private List<sp_Interface_GetImpSeasonsResult> _lstImpSeasons;
        private List<sp_Interface_GetSeasonsResult> _lstSeasons;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<string> _lstCollection;
        private AsyncCommand<object, bool> _getImpSeasons;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetImpSeasonsResult _impSeason;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addImpSeason;
        private ICommand _duplicate;
        public sp_Interface_GetImpSeasonsResult ImpSeason
        {
            get { return _impSeason; }
            set
            {
                _impSeason = value;
                PropertyChanged.Raise(this, o => o.ImpSeason);
            }
        }
        public AsyncCommand<object, bool> GetImpSeasons
        {
            get
            {
                if (_getImpSeasons == null)
                    _getImpSeasons = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadImpSeasons();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetImpSeasons.IsWorking; });
                return _getImpSeasons;
            }
        }

        public List<sp_Interface_GetImpSeasonsResult> LstImpSeasons
        {
            get { return _lstImpSeasons; }
            set
            {
                _lstImpSeasons = value;
                PropertyChanged.Raise(this, o => o.LstImpSeasons);
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
        public List<sp_Interface_GetFcstModelsResult> LstModel
        {
            get { return _lstModel; }
            set
            {
                _lstModel = value;
                PropertyChanged.Raise(this, o => o.LstModel);
            }

        }
        private bool LoadImpSeasons()
        {
            General.ChangeActivity(false);
            LstImpSeasons = _clsData.GetImpSeason();
            _lstSeasons = _clsData.GetSeasons();
            LstModel = _clsData.GetFcstModels();
            SelChangeExecute(LstImpSeasons.First());
            return true;
        }

        private void SelChangeExecute(object Parameter)
        {   
            if (Parameter != null && Parameter.ToString() != "")
            {
                sp_Interface_GetImpSeasonsResult p = ((sp_Interface_GetImpSeasonsResult)Parameter);
                LstCollection = p.Log_upd_user != ""? new List<string> { p.Fcst_collection }: UpdLstCollection(p.Fcst_model); 
                ImpSeason = new sp_Interface_GetImpSeasonsResult()
                {
                    Fcst_model = p.Fcst_model,
                    Fcst_collection = p.Fcst_collection,
                    Fcst_ImportActive=p.Fcst_ImportActive,
                    DueDateFrom=p.DueDateFrom,
                    DueDateTo=p.DueDateTo,
                    SFA_Collection=p.SFA_Collection,
                    Storedatefrom=p.Storedatefrom,
                    Storedateto=p.Storedateto,
                    DeadlineDate=p.DeadlineDate,
                    ForecastingUntilDate=p.ForecastingUntilDate,
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
            }
            else
            {
                _impSeason = new sp_Interface_GetImpSeasonsResult();
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null && ImpSeason.Log_upd_user == "")
            {
                LstCollection = UpdLstCollection(((sp_Interface_GetFcstModelsResult)Parameter).fcst_model);
            }
        }

        private List<string> UpdLstCollection(string FcstModel)
        {
            return _lstSeasons.Where(a => a.Fcst_model == FcstModel && a.Fcst_coll_Level == 0).Select(a => a.Fcst_collection).
                Except(LstImpSeasons.Where(a => a.Fcst_model == FcstModel).Select(a => a.Fcst_collection)).ToList();
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
        public ICommand AddImpSeason
        {
            get
            {
                if (_addImpSeason == null) _addImpSeason = new RelayCommand<object>(AddImpSeasonExecute);
                return _addImpSeason;
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
            sp_Interface_GetImpSeasonsResult xIS = ImpSeason;
            xIS.Log_upd_user = "";
            SelChangeExecute(LstImpSeasons.First());
            ImpSeason = xIS;
        }
        private void AddImpSeasonExecute(object Parameter)
        {
            if (ImpSeason.Log_upd_user=="" && LstImpSeasons.Any(a => a.Fcst_model == ImpSeason.Fcst_model && a.Fcst_collection == ImpSeason.Fcst_collection))
                MessageBox.Show("This season already exists", "Add Import Season", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (ImpSeason.Fcst_collection == "")
                {
                    MessageBox.Show("Please assign a Season", "Add Import Season", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _clsData.UpdImpSeason(ImpSeason);
                    _getImpSeasons.Execute(this);
                    MessageBox.Show("Your import Season has been saved.", "Import Season Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelChangeExecute(LstImpSeasons.First());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
