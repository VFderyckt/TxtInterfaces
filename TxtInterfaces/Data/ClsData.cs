using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TxtInterfaces.Data
{
    class ClsData
    {
        public List<sp_Interface_GetCfgSplitChannelResult> GetCfgSplitChannel()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetCfgSplitChannel().ToList();
        }
        public List<sp_Interface_GetCustomersDoorsResult> GetCustomersDoors()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetCustomersDoors().ToList();
        }
        public List<sp_Interface_GetProdCatResult> GetProdCat()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetProdCat().ToList();
        }
        public List<sp_Interface_GetEopsSeasonsResult> GetEopsSeason()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetEopsSeasons().ToList();
        }
        public List<sp_Interface_GetImpSeasonsResult> GetImpSeason()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetImpSeasons().ToList();
        }
        public List<sp_Interface_GetImportCustomersResult> GetImpCust()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetImportCustomers().ToList();
        }
        public List<sp_Interface_GetSAPvsTxtResult> GetSAPvsTxt()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetSAPvsTxt().ToList();
        }
        public List<sp_Interface_GetCountriesResult> GetCountries()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetCountries().ToList();
        }
        public List<sp_Interface_GetCustomersResult> GetCustomers()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetCustomers().ToList();
        }
        public List<sp_Interface_GetPlantsResult> GetPlants()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetPlants().ToList();
        }
        public List<sp_Interface_GetSeasonsResult> GetSeasons()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetSeasons().ToList();
        }
        public List<sp_Interface_GetXCountriesResult> GetXCountries()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetXCountries().ToList();
        }
        public List<sp_Interface_GetXCustomersResult> GetXCustomers()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetXCustomers().ToList();
        }
        public sp_Interface_GetParamsResult GetParams()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetParams(WindowsIdentity.GetCurrent().Name.Split('\\').Last()).FirstOrDefault();
        }
        public List<sp_Interface_GetFcstModelsResult> GetFcstModels()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetFcstModels().ToList();
        }
        public List<sp_Interface_GetDCsResult> GetDCs()
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            return ctx.sp_Interface_GetDCs().ToList();
        }
        public void UpdCountries(sp_Interface_GetCountriesResult Ctry)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdCountry(Ctry.Log_upd_user == "" || Ctry.Log_upd_user == null ? true : false, Ctry.Fcst_model, Ctry.CountryId,Ctry.CountryDesc,Ctry.Country_Level,Ctry.Country_FatherId,Ctry.CrossCountryId,Ctry.Currency_code,Ctry.Deleted_flag);
        }
        public void UpdCustomers(sp_Interface_GetCustomersResult Cus)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdCustomers(Cus.Log_upd_user == "" || Cus.Log_upd_user == null ? true : false, Cus.Fcst_model, Cus.Fcst_Customer,Cus.Fcst_CustomerD,Cus.Fcst_Cust_Level,Cus.Fcst_Cust_Father,Cus.Fcst_Excluded,Cus.Fcst_Cust_Type,Cus.Deleted_flag);
        }
        public void UpdPlant(sp_Interface_GetPlantsResult Plant)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdPlants(Plant.Log_upd_user == "" || Plant.Log_upd_user == null ? true : false, Plant.Fcst_model, Plant.Fcst_Plant, Plant.Priority,Plant.Fcst_DC,Plant.Deleted_flag);
        }
        public void UpdSeason(sp_Interface_GetSeasonsResult Ssn)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();            
            ctx.sp_Interface_UpdSeason(Ssn.Log_upd_user==""||Ssn.Log_upd_user == null?true:false,Ssn.Fcst_model,Ssn.Fcst_collection,Ssn.Fcst_collection_desc,Ssn.Fcst_coll_Level,Ssn.Fcst_coll_Father,Ssn.Priority,Ssn.SMU_flag,Ssn.Coll_previous,Ssn.Coll_Previous_Same_Season,Ssn.Deleted_flag);
        }
        public void UpdXCustomer(sp_Interface_GetXCustomersResult XCus)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdXCustomers(XCus.Log_upd_user == "" || XCus.Log_upd_user == null ? 1 : XCus.Log_upd_user == "Del" ? 2 : 0, XCus.Fcst_model,XCus.Customer_code,XCus.Customer_door,XCus.CrossCustomer_code,XCus.CrossCustomer_door,XCus.CrossCustomer_country,XCus.Deleted_flag);
        }
        public void UpdSAPvsTxt(sp_Interface_GetSAPvsTxtResult SapvsTxt,bool Add)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdSAPvsTxt(Add ? 1 : 0, SapvsTxt.Fcst_model, SapvsTxt.SAPReqSegment,SapvsTxt.SAPPlant,SapvsTxt.SAPSsnYr,SapvsTxt.SAPSsn,SapvsTxt.TxtCollection,SapvsTxt.TxtChannel,SapvsTxt.TxtOrderType,SapvsTxt.TxtCustomer,SapvsTxt.ExportToSAP,SapvsTxt.SAPExtRegId,SapvsTxt.FlagAlignSO,SapvsTxt.FLagFcst,SapvsTxt.DemandOffSetDays,SapvsTxt.UsePlant,SapvsTxt.Fcst_DC);
        }
        public void UpdImpCust(sp_Interface_GetImportCustomersResult ImpCust)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdImportCustomers(ImpCust.Log_upd_user == "" ? 1 : ImpCust.Log_upd_user=="Del" ? 2:0, ImpCust.Fcst_Model, ImpCust.Fcst_Priority, ImpCust.Fcst_Customer, ImpCust.Customer_code,
                ImpCust.Delivery_addresscode, ImpCust.Collection_From, ImpCust.Collection_To, ImpCust.Product_Category_From,
                ImpCust.Product_Category_To, ImpCust.Division, ImpCust.SalesRegion, ImpCust.CCSalesRegion, ImpCust.ClassIDFrom, ImpCust.ClassIDto,
                ImpCust.Definition_level, ImpCust.Comment, ImpCust.Deleted_Flag);
        }
        public void UpdImpSeason(sp_Interface_GetImpSeasonsResult ImpSsn)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdImpSeason(ImpSsn.Log_upd_user == "" ? true : false, ImpSsn.Fcst_model,ImpSsn.Fcst_collection,ImpSsn.Fcst_ImportActive,ImpSsn.DueDateFrom,
                ImpSsn.DueDateTo,ImpSsn.SFA_Collection,ImpSsn.Storedatefrom,ImpSsn.Storedateto,ImpSsn.DeadlineDate,ImpSsn.ForecastingUntilDate,ImpSsn.Deleted_flag);
        }
        public void UpdCfgSplitChannel(sp_Interface_GetCfgSplitChannelResult CfgSplitChannel)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_UpdCfgSplitChannel(CfgSplitChannel.Log_upd_user == "" ? true : false, CfgSplitChannel.Fcst_model, CfgSplitChannel.SplitChannel, CfgSplitChannel.MainChannel,CfgSplitChannel.Deleted_flag);
        }
        public void CalcEops (string FMod,string FCol,string Prodcat,int Repl)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext
            {
                CommandTimeout = 0
            };
            ctx.sp_Calc_EOPS(FMod, FCol, Prodcat, Repl, 0);
        }
        public void CopyCfgTxtvsSap(sp_Interface_GetSAPvsTxtResult TvS,string Cfrom)
        {
            TxtIfacesDataContext ctx = new TxtIfacesDataContext();
            ctx.sp_Interface_CopyCfgTxtvsSap(TvS.Fcst_model,TvS.SAPSsnYr,TvS.TxtCollection,Cfrom);
        }
    }
}
