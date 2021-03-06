﻿//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 1.1.4322.573.
// 
namespace FrontDesk.Applications.Submission.SubmissionService {
    using System.Diagnostics;
    using System.Xml.Serialization;
    using System;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Web.Services;
	using FrontDesk.Applications.Submission.UserService;
    
    
    /// <remarks/>
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SubmissionServiceSoap", Namespace="http://FrontDesk/WebServices")]
    public class SubmissionService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        public ServiceTicket ServiceTicketValue;
        
        /// <remarks/>
        public SubmissionService() {
            this.Url = "http://localhost/FrontDeskServices/subsvc.asmx";
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceTicketValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://FrontDesk/WebServices/ZipArchiveSubmit", RequestNamespace="http://FrontDesk/WebServices", ResponseNamespace="http://FrontDesk/WebServices", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void ZipArchiveSubmit([System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] System.Byte[] zipdata, int principalID, int asstID) {
            this.Invoke("ZipArchiveSubmit", new object[] {
                        zipdata,
                        principalID,
                        asstID});
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginZipArchiveSubmit(System.Byte[] zipdata, int principalID, int asstID, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ZipArchiveSubmit", new object[] {
                        zipdata,
                        principalID,
                        asstID}, callback, asyncState);
        }
        
        /// <remarks/>
        public void EndZipArchiveSubmit(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://FrontDesk/WebServices/Authenticate", RequestNamespace="http://FrontDesk/WebServices", ResponseNamespace="http://FrontDesk/WebServices", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public ServiceTicket Authenticate(string username, string password) {
            object[] results = this.Invoke("Authenticate", new object[] {
                        username,
                        password});
            return ((ServiceTicket)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginAuthenticate(string username, string password, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Authenticate", new object[] {
                        username,
                        password}, callback, asyncState);
        }
        
        /// <remarks/>
        public ServiceTicket EndAuthenticate(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((ServiceTicket)(results[0]));
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ServiceTicketValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://FrontDesk/WebServices/Logout", RequestNamespace="http://FrontDesk/WebServices", ResponseNamespace="http://FrontDesk/WebServices", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void Logout() {
            this.Invoke("Logout", new object[0]);
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginLogout(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Logout", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public void EndLogout(System.IAsyncResult asyncResult) {
            this.EndInvoke(asyncResult);
        }
    }
    

}
