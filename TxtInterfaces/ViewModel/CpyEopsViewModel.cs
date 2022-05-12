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
    class CpyEopsViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetEopsSeasonsResult> _lstSsn;
        private List<sp_Interface_GetEopsSeasonsResult> _lstSeasons;
        private List<sp_Interface_GetProdCatResult> _lstPCat;
        private List<string> _lstProdCat;
        private sp_Interface_GetEopsSeasonsResult _selSsn;
        private ClsData _clsData = new ClsData();
        private ICommand _selChangeModel;
        private ICommand _cmdCreate;
        private string _selMod;
        private string _selProdCat;
        private AsyncCommand<object, bool> _getCpyEops;
        public AsyncCommand<object, bool> GetCpyEops
        {
            get
            {
                if (_getCpyEops == null)
                    _getCpyEops = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadCpyEops();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetCpyEops.IsWorking; });
                return _getCpyEops;
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
        public List<String> LstProdCat
        {
            get { return _lstProdCat; }
            set { _lstProdCat = value;
                PropertyChanged.Raise(this, o => o.LstProdCat);
            }
        }
        public List<sp_Interface_GetEopsSeasonsResult> LstSeasons
        {
            get { return _lstSeasons; }
            set
            {
                _lstSeasons = value;
                PropertyChanged.Raise(this, o => o.LstSeasons);
            }
        }
        public sp_Interface_GetEopsSeasonsResult SelSsn
        {
            get { return _selSsn; }
            set
            {
                _selSsn = value;
                PropertyChanged.Raise(this, o => o.SelSsn);
            }
        }
        public string SelMod
        {
            get { return _selMod; }
            set
            {
                _selMod = value;
                PropertyChanged.Raise(this, o => o.SelMod);
            }
        }
        public string SelProdCat
        {
            get { return _selProdCat; }
            set
            {
                _selProdCat = value;
                PropertyChanged.Raise(this, o => o.SelProdCat);
            }
        }
        private bool LoadCpyEops()
        {
            General.ChangeActivity(false);
            LstModel = _clsData.GetFcstModels();
            _lstSsn = _clsData.GetEopsSeason();
            _lstPCat = _clsData.GetProdCat();
            SelMod = LstModel.FirstOrDefault().fcst_model;
            return true;
        }
        private void SelChangeModelExecute(object Parameter)
        {
            if (Parameter != null)
            {
                sp_Interface_GetFcstModelsResult x = (sp_Interface_GetFcstModelsResult)Parameter;
                LstSeasons = _lstSsn.Where(a => a.fcst_model == x.fcst_model).ToList();
                SelSsn = LstSeasons.FirstOrDefault();
                LstProdCat = _lstPCat.Where(a => a.Fcst_model == x.fcst_model).Select(a => a.Prodcat).ToList();
                SelProdCat = LstProdCat.FirstOrDefault();
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
        public ICommand CmdCreate
        {
            get
            {
                if (_cmdCreate == null) _cmdCreate = new RelayCommand<object>(CmdCreateExecute);
                return _cmdCreate;
            }
        }
        private void CmdCreateExecute(object Parameter)
        {
            int repl=SelSsn.LastDate==null?0:MessageBox.Show("Are you sure you want to overwrite existing version ?", "Create EOPS", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes?1:2;
            if (repl < 2)
            {
                General.ChangeActivity(false);
                _clsData.CalcEops(SelMod, SelSsn.Fcst_collection, SelProdCat, repl);
                MessageBox.Show("EOPS created for " + SelMod + " " + SelSsn.Fcst_collection + " " + SelProdCat, "Create EOPS", MessageBoxButton.OK, MessageBoxImage.Information);
                General.ChangeActivity(true);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
