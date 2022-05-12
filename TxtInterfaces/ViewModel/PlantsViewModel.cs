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
    class PlantsViewModel : INotifyPropertyChanged
    {
        private List<sp_Interface_GetPlantsResult> _lstPlants;
        private List<sp_Interface_GetFcstModelsResult> _lstModel;
        private List<sp_Interface_GetDCsResult> _lstDCs;
        private AsyncCommand<object, bool> _getPlants;
        private ClsData _clsData = new ClsData();
        private sp_Interface_GetPlantsResult _Plant;
        private ICommand _selChange;
        private ICommand _addPlant;
        private ICommand _duplicate;
        public sp_Interface_GetPlantsResult Plant
        {
            get { return _Plant; }
            set
            {
                _Plant = value;
                PropertyChanged.Raise(this, o => o.Plant);
            }
        }
        public AsyncCommand<object, bool> GetPlants
        {
            get
            {
                if (_getPlants == null)
                    _getPlants = new AsyncCommand<object, bool>((po) =>
                    {
                        return LoadPlants();
                    },
                        (pb) =>
                        {
                            General.ChangeActivity(true);
                        },
                        () => { return !GetPlants.IsWorking; });
                return _getPlants;
            }
        }

        public List<sp_Interface_GetPlantsResult> LstPlants
        {
            get { return _lstPlants; }
            set
            {
                _lstPlants = value;
                PropertyChanged.Raise(this, o => o.LstPlants);
            }
        }
        public List<sp_Interface_GetDCsResult> LstDCs
        {
            get { return _lstDCs; }
            set
            {
                _lstDCs = value;
                PropertyChanged.Raise(this, o => o.LstDCs);
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
        private bool LoadPlants()
        {
            General.ChangeActivity(false);
            LstPlants = _clsData.GetPlants();
            LstModel = _clsData.GetFcstModels();
            LstDCs = _clsData.GetDCs();
            SelChangeExecute(LstPlants.First());
            return true;
        }

        private void SelChangeExecute(object Parameter)
        {

            sp_Interface_GetPlantsResult p = ((sp_Interface_GetPlantsResult)Parameter);
            if (p != null)
            {
                Plant = new sp_Interface_GetPlantsResult()
                {
                    Fcst_model = p.Fcst_model,
                    Fcst_Plant = p.Fcst_Plant,
                    Priority = p.Priority,
                    Fcst_DC = p.Fcst_DC,
                    Deleted_flag = p.Deleted_flag,
                    Log_upd_user = p.Log_upd_user
                };
            }
            else
            {
                _Plant = new sp_Interface_GetPlantsResult();
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
        public ICommand AddPlant
        {
            get
            {
                if (_addPlant == null) _addPlant = new RelayCommand<object>(AddPlantExecute);
                return _addPlant;
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
            sp_Interface_GetPlantsResult xPla = Plant;
            xPla.Log_upd_user = "";
            SelChangeExecute(LstPlants.First());
            Plant = xPla;
        }
        private void AddPlantExecute(object Parameter)
        {
            if (Plant.Log_upd_user=="" && LstPlants.Any(a => a.Fcst_model == Plant.Fcst_model && a.Fcst_Plant == Plant.Fcst_Plant))
                MessageBox.Show("This plant is already setup", "Add Plant", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (Plant.Fcst_Plant == "" ||Plant.Fcst_DC=="")
                {
                    MessageBox.Show("Please assign a Plant and a Fcst_DC", "Add Plant", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _clsData.UpdPlant(Plant);
                    _getPlants.Execute(this);
                    MessageBox.Show("Your plant has been saved.", "Plant Setup", MessageBoxButton.OK, MessageBoxImage.Information);
                    SelChangeExecute(LstPlants.First());
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
