﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TxtInterfaces.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("|FR|KA|OTH")]
        public string CustTypes {
            get {
                return ((string)(this["CustTypes"]));
            }
            set {
                this["CustTypes"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=veat2atxtsqlqa\\sql_whs_qa;Initial Catalog=VFTXT_IFACES;Integrated Sec" +
            "urity=True")]
        public string VFTXT_IFACESConnectionString1 {
            get {
                return ((string)(this["VFTXT_IFACESConnectionString1"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@TIM|DIS_PB|DIS_RO|WHS_PB|WHS_RO@VAN|WHS_PB|WHS_RO@TTV|DIS_PB|DIS_RO|WHS_PB|WHS_R" +
            "O")]
        public string SAPReqSeg {
            get {
                return ((string)(this["SAPReqSeg"]));
            }
            set {
                this["SAPReqSeg"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@TIM|45FW|45SS@VAN|30F0|50F0|30H0|50H0|30S0|50S0@TTV|45FW|45SS")]
        public string SAPSsn {
            get {
                return ((string)(this["SAPSsn"]));
            }
            set {
                this["SAPSsn"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@TIM|DE|DS|WH@VAN|DS|WH@TTV|DE|DS|WH")]
        public string TxtChannel {
            get {
                return ((string)(this["TxtChannel"]));
            }
            set {
                this["TxtChannel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@TIM|PS|RO@VAN|PS|RO@TTV|PS|RO")]
        public string TxtOrderType {
            get {
                return ((string)(this["TxtOrderType"]));
            }
            set {
                this["TxtOrderType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("@TIM|DE_OTHER|DS_OTHER|WH_OTHER@VAN|DS_OTHER|WH_OTHER@TTV|DE_OTHER|DS_OTHER|WH_OT" +
            "HER")]
        public string TxtCust {
            get {
                return ((string)(this["TxtCust"]));
            }
            set {
                this["TxtCust"] = value;
            }
        }
    }
}
