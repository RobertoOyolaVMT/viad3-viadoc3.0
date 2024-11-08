﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace ViaDocEnvioCorreo.Negocios.WsPortalFacturacionElectronica {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="wsdocumentoBinding", Namespace="urn:wsdocumento")]
    public partial class wsdocumento : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback insertarOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public wsdocumento() {
            this.Url = global::ViaDocEnvioCorreo.Negocios.Properties.Settings.Default.ViaDocEnvioCorreo_Negocios_WsPortalFacturacionElectronica_wsdocumento;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event insertarCompletedEventHandler insertarCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("urn:wsdocumento#insertar", RequestNamespace="urn:insertar", ResponseNamespace="urn:insertar")]
        [return: System.Xml.Serialization.SoapElementAttribute("respuesta")]
        public string insertar(string ruc_empresa, string tip_documento, string num_documento, string cedruc_cliente, string tipo_emision, string num_autorizacion, string xml_autorizado, string fec_emision, string fec_autorizacion, string ciContingenciaDet) {
            object[] results = this.Invoke("insertar", new object[] {
                        ruc_empresa,
                        tip_documento,
                        num_documento,
                        cedruc_cliente,
                        tipo_emision,
                        num_autorizacion,
                        xml_autorizado,
                        fec_emision,
                        fec_autorizacion,
                        ciContingenciaDet});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void insertarAsync(string ruc_empresa, string tip_documento, string num_documento, string cedruc_cliente, string tipo_emision, string num_autorizacion, string xml_autorizado, string fec_emision, string fec_autorizacion, string ciContingenciaDet) {
            this.insertarAsync(ruc_empresa, tip_documento, num_documento, cedruc_cliente, tipo_emision, num_autorizacion, xml_autorizado, fec_emision, fec_autorizacion, ciContingenciaDet, null);
        }
        
        /// <remarks/>
        public void insertarAsync(string ruc_empresa, string tip_documento, string num_documento, string cedruc_cliente, string tipo_emision, string num_autorizacion, string xml_autorizado, string fec_emision, string fec_autorizacion, string ciContingenciaDet, object userState) {
            if ((this.insertarOperationCompleted == null)) {
                this.insertarOperationCompleted = new System.Threading.SendOrPostCallback(this.OninsertarOperationCompleted);
            }
            this.InvokeAsync("insertar", new object[] {
                        ruc_empresa,
                        tip_documento,
                        num_documento,
                        cedruc_cliente,
                        tipo_emision,
                        num_autorizacion,
                        xml_autorizado,
                        fec_emision,
                        fec_autorizacion,
                        ciContingenciaDet}, this.insertarOperationCompleted, userState);
        }
        
        private void OninsertarOperationCompleted(object arg) {
            if ((this.insertarCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.insertarCompleted(this, new insertarCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void insertarCompletedEventHandler(object sender, insertarCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class insertarCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal insertarCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591