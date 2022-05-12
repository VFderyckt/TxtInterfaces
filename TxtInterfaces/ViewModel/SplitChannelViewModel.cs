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
    class SplitChannelViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetCfgSplitChannelResult> _lstSplitChannels;
        private List<string> _lstChannels;
        private List<string> _lstMChannels;
        private List<sp_Interface_GetCustomersResult> _lstCust;
        private AsyncCommand<object, bool> _getSplitChannel;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetCfgSplitChannelResult _splitChannel;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addSplitChannel;
        public List<sp_Interface_GetFcstModelsResult> LstModel
        {
            get { return _lstModel; }
            set { _lstModel = value;
                PropertyChanged.Raise(this, o => o.LstModel);
            }
        }
        public ICommand AddSplitChannel
        {
            get
            {
                if (_addSplitChannel == null) _addSplitChannel = new RelayCommand<object>(AddSplitChannelExecute);
                return _addSplitChannel;
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
        public List<sp_Interface_GetCfgSplitChannelResult> LstSplitChannels
        {
            get { return _lstSplitChannels; }
            set { _lstSplitChannels = value;
                PropertyChanged.Raise(this, o => o.LstSplitChannels); }
        }
        public List<string> LstChannels
        {
            get {return _lstChannels;}
            set { _lstChannels = value;
                PropertyChanged.Raise(this, o => o.LstChannels);
            }
        }
        public List<string> LstMChannels
        {
            get { return _lstMChannels; }
            set
            {
                _lstMChannels = value;
                PropertyChanged.Raise(this, o => o.LstMChannels);
            }
        }
        public sp_Interface_GetCfgSplitChannelResult SplitChannel
        {
            get { return _splitChannel; }
            set { _splitChannel = value;
                PropertyChanged.Raise(this, o => o.SplitChannel); }
        }
        public AsyncCommand<object, bool> GetSplitChannel
        {
            get
            {
                if (_getSplitChannel == null)
                    _getSplitChannel = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadSplitChannels();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetSplitChannel.IsWorking; });
                return _getSplitChannel;
            }
        }
        private bool LoadSplitChannels()
        {
            General.ChangeActivity(false);
            _lstCust = _clsData.GetCustomers();
            LstSplitChannels = _clsData.GetCfgSplitChannel();
            LstModel = _clsData.GetFcstModels();
            SelChangeExecute(LstSplitChannels.First());
            return true;
        }
        private void SelChangeExecute(object Parameter)
        {
            sp_Interface_GetCfgSplitChannelResult p = ((sp_Interface_GetCfgSplitChannelResult)Parameter);
            SplitChannel=p==null? new sp_Interface_GetCfgSplitChannelResult(): new sp_Interface_GetCfgSplitChannelResult() { Fcst_model = p.Fcst_model, SplitChannel = p.SplitChannel, MainChannel = p.MainChannel, Log_upd_user = p.Log_upd_user,Deleted_flag=p.Deleted_flag };
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                string fMod = ((sp_Interface_GetFcstModelsResult)Parameter).fcst_model;
                LstMChannels = GetSettingsList(Properties.Settings.Default.TxtChannel, fMod);
                LstChannels = _lstCust.Where(a => a.Fcst_model == fMod && a.Fcst_Excluded != 1 && a.Deleted_flag != 1 && a.Fcst_Cust_Level==2).Select(a => a.Fcst_Customer).ToList();
                LstChannels.RemoveAll(a => LstMChannels.Exists(w =>w == a));
            }
        }
        private void AddSplitChannelExecute(object Parameter)
        {
            if (SplitChannel.Log_upd_user == "" && LstSplitChannels.Any(a => a.Fcst_model == SplitChannel.Fcst_model && a.SplitChannel == SplitChannel.SplitChannel))
                MessageBox.Show("This Channel is already setup", "Add SplitChannel", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (SplitChannel.MainChannel == "")
                {
                    MessageBox.Show("Please assign a Main Channel", "Add SplitChannel", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _clsData.UpdCfgSplitChannel(SplitChannel);
                    _getSplitChannel.Execute(this);
                    MessageBox.Show("Your Configuration has been saved.", "SplitChannel Configuration", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelChangeExecute(LstSplitChannels.First());
                }
            }
        }
        private List<string> GetSettingsList(string set, string fMod)
        {
            return set.Split('@').FirstOrDefault(s => s.Split('|')[0] == fMod)?.Replace(fMod + "|", "").Split('|').ToList();
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
