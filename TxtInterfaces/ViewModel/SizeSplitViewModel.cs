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
    class SizeSplitViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetSizeSplitResult> _lstSizeSplit;
        private AsyncCommand<object, bool> _getSizeSplit;
        private ClsData _clsData = new ClsData();
        private List<string> _lstCollection;
        private List<string> _lstCollectionHistory;
        private List<sp_Interface_GetSeasonsResult> _lstSeasons;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private sp_Interface_GetSizeSplitResult _sizeSplit;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addSizeSplit;

        public AsyncCommand<object, bool> GetSizeSplit
        {
            get
            {
                if (_getSizeSplit == null)
                    _getSizeSplit = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadSizeSplit();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetSizeSplit.IsWorking; });
                return _getSizeSplit;
            }
        }
        private bool LoadSizeSplit()
        {
            General.ChangeActivity(false);
            LstSizeSplit = _clsData.GetSizeSplit();
            LstModel = _clsData.GetFcstModels();
            _lstSeasons = _clsData.GetSeasons();
            SelChangeExecute(LstSizeSplit.First());
            return true;
        }
        public sp_Interface_GetSizeSplitResult SizeSplit
        {
            get { return _sizeSplit; }
            set
            {
                _sizeSplit = value;
                PropertyChanged.Raise(this, o => o.SizeSplit);
            }
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                sp_Interface_GetFcstModelsResult x = (sp_Interface_GetFcstModelsResult)Parameter;
                LstCollectionHistory = _lstSeasons.Where(a => a.Fcst_model == x.fcst_model && a.Fcst_coll_Level == 0).Select(a => a.Fcst_collection).ToList();
                if (SizeSplit.Log_upd_user=="") LstCollection = UpdCollectionlist(x.fcst_model);
            }
        }

        private List<string> UpdCollectionlist(string FcstModel)
        {
            return _lstSeasons.Where(a => a.Fcst_model == FcstModel && a.Fcst_coll_Level == 0).Select(a => a.Fcst_collection).ToList();
        }
        private void SelChangeExecute(object Parameter)
        {
            if (Parameter != null && Parameter.ToString() != "")
            {
                sp_Interface_GetSizeSplitResult p = ((sp_Interface_GetSizeSplitResult)Parameter);
                LstCollection = p.Log_upd_user != ""? new List<string> { p.Collection }: UpdCollectionlist(p.Fcst_model);
                SizeSplit = new sp_Interface_GetSizeSplitResult()
                {
                    Fcst_model = p.Fcst_model,
                    Collection = p.Collection,
                    Collection_History = p.Collection_History,
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
            }
            else
            {
                _sizeSplit = new sp_Interface_GetSizeSplitResult();
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
        public List<sp_Interface_GetSizeSplitResult> LstSizeSplit
        {
            get { return _lstSizeSplit; }
            set
            {
                _lstSizeSplit = value;
                PropertyChanged.Raise(this, o => o.LstSizeSplit);
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
        public List<string> LstCollectionHistory
        {
            get { return _lstCollectionHistory; }
            set
            {
                _lstCollectionHistory = value;
                PropertyChanged.Raise(this, o => o.LstCollectionHistory);
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
        public ICommand AddSizeSplit
        {
            get
            {
                if (_addSizeSplit == null) _addSizeSplit = new RelayCommand<object>(AddSizeSplitExecute);
                return _addSizeSplit;
            }
        }
        private void AddSizeSplitExecute(object Parameter)
        {
            if (SizeSplit.Collection == "")
            {
                MessageBox.Show("Please assign a Collection", "Add SizeSplit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (SizeSplit.Collection_History == "")
                {
                    MessageBox.Show("Please assign History Collection", "Add SizeSplit", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (SizeSplit.Log_upd_user == "" && LstSizeSplit.Any(a => a.Fcst_model == SizeSplit.Fcst_model && a.Collection == SizeSplit.Collection && a.Collection_History == SizeSplit.Collection_History))
                    {
                        MessageBox.Show("Combination Collection/History Collection already exists", "Add SizeSplit", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else 
                    {
                        _clsData.UpdSizeSplit(SizeSplit);
                        _getSizeSplit.Execute(this);
                        MessageBox.Show("Your SizeSplit has been saved.", "Add/Update SizeSplit", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelChangeExecute(LstSizeSplit.First());
                    }
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
