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
    class SAPvsTxtViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetSAPvsTxtResult> _lstSAPvsTxt;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetPlantsResult> _lstPla;
        private List<sp_Interface_GetSeasonsResult> _lstTSsn;
        private List<string> _lstSAPReqSeg;
        private List<string> _lstSAPSsn;
        private List<string> _lstTxtSsn;
        private List<string> _lstPlants;
        private List<string> _lstTxtOT;
        private List<string> _lstTxtCust;
        private List<string> _lstFrom;
        private List<int> _lstYr;
        private List<string> _lstChannel;
        private bool _badd;
        private AsyncCommand<object, bool> _getSAPvsTxt;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetSAPvsTxtResult _sAPvsTxt;
        private string _ssnFrom;
        private bool _chkCopy = false;
        private ICommand _selChange;
        private ICommand _selChangeModel;
        private ICommand _addSAPvsTxt;

        public string SsnFrom
        {
            get { return _ssnFrom; }
            set
            {
                _ssnFrom = value;
                PropertyChanged.Raise(this, o => o.SsnFrom);
            }
        }
        public bool ChkCopy
        {
            get { return _chkCopy; }
            set
            {
                _chkCopy = value;
                PropertyChanged.Raise(this, o => o.ChkCopy);
            }
        }
        public List<sp_Interface_GetSAPvsTxtResult> LstSAPvsTxt
        {
            get { return _lstSAPvsTxt; }
            set
            {
                _lstSAPvsTxt = value;
                PropertyChanged.Raise(this, o => o.LstSAPvsTxt);
            }
        }
        public List<string> LstPlants
        {
            get { return _lstPlants; }
            set
            {
                _lstPlants = value;
                PropertyChanged.Raise(this, o => o.LstPlants);
            }
        }
        public List<string> LstTxtOT
        {
            get { return _lstTxtOT; }
            set
            {
                _lstTxtOT = value;
                PropertyChanged.Raise(this, o => o.LstTxtOT);
            }
        }
        public List<string> LstTxtCust
        {
            get { return _lstTxtCust; }
            set
            {
                _lstTxtCust = value;
                PropertyChanged.Raise(this, o => o.LstTxtCust);
            }
        }
        public List<string> LstChannel
        {
            get { return _lstChannel; }
            set
            {
                _lstChannel = value;
                PropertyChanged.Raise(this, o => o.LstChannel);
            }
        }
        public List<int> LstYr
        {
            get { return _lstYr; }
            set
            {
                _lstYr = value;
                PropertyChanged.Raise(this, o => o.LstYr);
            }
        }
        public List<string> LstSAPReqSeg
        {
            get { return _lstSAPReqSeg; }
            set
            {
                _lstSAPReqSeg  = value;
                PropertyChanged.Raise(this, o => o.LstSAPReqSeg);
            }
        }
        public List<string> LstSAPSsn
        {
            get { return _lstSAPSsn; }
            set
            {
                _lstSAPSsn = value;
                PropertyChanged.Raise(this, o => o.LstSAPSsn);
            }
        }
        public List<string> LstTxtSsn
        {
            get { return _lstTxtSsn; }
            set
            {
                _lstTxtSsn = value;
                PropertyChanged.Raise(this, o => o.LstTxtSsn);
            }
        }
        public List<string> LstFrom
        {
            get { return _lstFrom; }
            set
            {
                _lstFrom = value;
                PropertyChanged.Raise(this, o => o.LstFrom);
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
        public sp_Interface_GetSAPvsTxtResult SAPvsTxt
        {
            get { return _sAPvsTxt; }
            set
            {
                _sAPvsTxt = value;
                PropertyChanged.Raise(this, o => o.SAPvsTxt);
            }
        }
        public AsyncCommand<object, bool> GetSAPvsTxt
        {
            get
            {
                if (_getSAPvsTxt == null)
                    _getSAPvsTxt = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadSAPvsTxt();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetSAPvsTxt.IsWorking; });
                return _getSAPvsTxt;
            }
        }
        private bool LoadSAPvsTxt()
        {
            General.ChangeActivity(false);
            LstSAPvsTxt = _clsData.GetSAPvsTxt();
            LstModel = _clsData.GetFcstModels();
            _lstPla = _clsData.GetPlants();
            _lstTSsn = _clsData.GetSeasons();
            SelChangeExecute(LstSAPvsTxt.First());
            return true;
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                string fMod = ((sp_Interface_GetFcstModelsResult)Parameter).fcst_model;
                LstPlants =_lstPla.Where(a => a.Fcst_model == fMod && a.Fcst_Plant != "").Select(a => a.Fcst_Plant).ToList();
                LstSAPReqSeg = GetSettingsList(Properties.Settings.Default.SAPReqSeg, fMod);
                LstSAPSsn = GetSettingsList(Properties.Settings.Default.SAPSsn, fMod);
                LstChannel = GetSettingsList(Properties.Settings.Default.TxtChannel, fMod);
                LstTxtOT = GetSettingsList(Properties.Settings.Default.TxtOrderType, fMod);
                LstTxtCust = GetSettingsList(Properties.Settings.Default.TxtCust, fMod);
                LstTxtSsn = _lstTSsn.Where(a => a.Fcst_model == fMod && a.Fcst_coll_Level==0).Select(a => a.Fcst_collection).ToList();
                LstFrom = _lstSAPvsTxt.Where(a=> a.Fcst_model == fMod && a.TxtCollection!="").Select(a => a.TxtCollection).Distinct().ToList();
                int yr = (int)_lstSAPvsTxt.Where(a => a.Fcst_model == "xxx").Select(a => a.SAPSsnYr).DefaultIfEmpty(DateTime.Now.Year - 2).Min();
                _lstYr = new List<int>();
                for (int i = yr; i <= DateTime.Now.Year + 3;i++)
                    _lstYr.Add(i);
                LstYr = _lstYr;
            }
        }
        private List<string> GetSettingsList(string set, string fMod)
        {
            return set.Split('@').FirstOrDefault(s => s.Split('|')[0] == fMod)?.Replace(fMod + "|", "").Split('|').ToList();
        }
        private void SelChangeExecute(object Parameter)
        {
            sp_Interface_GetSAPvsTxtResult p = ((sp_Interface_GetSAPvsTxtResult)Parameter);
            if (p != null)
            {
                SAPvsTxt = new sp_Interface_GetSAPvsTxtResult()
                {
                    Fcst_model = p.Fcst_model,
                    SAPReqSegment = p.SAPReqSegment,
                    SAPPlant = p.SAPPlant,
                    SAPSsnYr = p.SAPSsnYr,
                    SAPSsn = p.SAPSsn,
                    TxtCollection = p.TxtCollection,
                    TxtChannel = p.TxtChannel,
                    TxtOrderType = p.TxtOrderType,
                    TxtCustomer = p.TxtCustomer,
                    ExportToSAP = p.ExportToSAP,
                    SAPExtRegId = p.SAPExtRegId,
                    FlagAlignSO = p.FlagAlignSO,
                    FLagFcst = p.FLagFcst,
                    DemandOffSetDays = p.DemandOffSetDays,
                    UsePlant = p.UsePlant,
                    Fcst_DC = p.Fcst_DC
                };
                _badd = p.SAPReqSegment=="";
            }
            else
            {
                _sAPvsTxt = new sp_Interface_GetSAPvsTxtResult();
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
        public ICommand SelChange
        {
            get
            {
                if (_selChange == null) _selChange = new RelayCommand<object>(SelChangeExecute);
                return _selChange;
            }
        }
        public ICommand AddSAPvsTxt
        {
            get
            {
                if (_addSAPvsTxt == null) _addSAPvsTxt = new RelayCommand<object>(AddSAPvsTxtExecute);
                return _addSAPvsTxt;
            }
        }
        private void AddSAPvsTxtExecute(object Parameter)
        {
            if (_badd && ChkCopy)
                CopyFrom();
            else
            {
                if (_badd && LstSAPvsTxt.Any(a => a.Fcst_model == SAPvsTxt.Fcst_model && a.SAPPlant == SAPvsTxt.SAPPlant && a.SAPReqSegment == SAPvsTxt.SAPReqSegment
                     && a.SAPSsn == SAPvsTxt.SAPSsn && a.SAPSsnYr == SAPvsTxt.SAPSsnYr))
                    MessageBox.Show("There is already a setup present for this combination", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    if (SAPvsTxt.SAPReqSegment == "" || SAPvsTxt.SAPPlant == "" || SAPvsTxt.SAPSsnYr < 2016 || SAPvsTxt.SAPSsn == "")
                    {
                        MessageBox.Show("Please assign SAP Requirement Segment, SAP Plant, SAP Year and SAP Season", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        //set sap fcst_dc
                        SAPvsTxt.Fcst_DC = _lstPla.Where(a => a.Fcst_model == SAPvsTxt.Fcst_model && a.Fcst_Plant == SAPvsTxt.SAPPlant).Select(a => a.Fcst_DC).FirstOrDefault().ToString();
                        string _curDefPlant = ((bool)SAPvsTxt.ExportToSAP) ? LstSAPvsTxt.Where(a => a.Fcst_model == SAPvsTxt.Fcst_model && a.TxtChannel == SAPvsTxt.TxtChannel &&
                              a.TxtOrderType == SAPvsTxt.TxtOrderType && a.TxtCollection == SAPvsTxt.TxtCollection && (bool)a.ExportToSAP && a.Fcst_DC==SAPvsTxt.Fcst_DC).Select(a => a.SAPPlant).FirstOrDefault() : SAPvsTxt.SAPPlant;

                        if (((_curDefPlant != "" && _curDefPlant != SAPvsTxt.SAPPlant) ? MessageBox.Show("Default Plant will be changed from " + _curDefPlant + " to " + SAPvsTxt.SAPPlant, "Change Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) : MessageBoxResult.Yes)==MessageBoxResult.Yes)
                        { 
                            _clsData.UpdSAPvsTxt(SAPvsTxt, _badd);
                            Reset();
                        }
                    }
                }
            }
        }
        private void CopyFrom()
        {
            //check if from collection is not null
            if (SsnFrom == null || SsnFrom == "")
                MessageBox.Show("Please select a collection to copy from", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                //check if TXTCollection <> CollectionFrom
                if (SsnFrom == SAPvsTxt.TxtCollection)
                    MessageBox.Show("TXTCollection to generate can't be the same as the collection to copy from.  \r\nPlease choose different one", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    //check mandatory fields
                    if(SAPvsTxt.TxtCollection=="")
                        MessageBox.Show("Please assign TxtCollection", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        if (_lstSAPvsTxt.Any(a=>a.Fcst_model==SAPvsTxt.Fcst_model && a.SAPSsn==SAPvsTxt.SAPSsn && a.SAPSsnYr==SAPvsTxt.SAPSsnYr && a.TxtCollection!=SAPvsTxt.TxtCollection))
                            MessageBox.Show("There is already another TxtCollection assigned to this SAP Season and SAP SeasonYear ", "Add SAPvsTxt", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        {
                            if (MessageBox.Show("Are you sure you want to copy settings from " + SsnFrom + " to " + SAPvsTxt.TxtCollection + "\r\n Existing settings will be overwritten !", "Add SAPvsTxt", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                _clsData.CopyCfgTxtvsSap(SAPvsTxt, SsnFrom);
                                Reset();
                            }
                        }                        
                    }
                }
            }
        }
        private void Reset()
        {
            _getSAPvsTxt.Execute(this);
            MessageBox.Show("Your configuration has been saved.", "SAPvsTxt Setup", MessageBoxButton.OK, MessageBoxImage.Information);
            SelChangeExecute(LstSAPvsTxt.First());
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
