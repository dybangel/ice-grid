﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace autosell_center.WXPay {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="https://www.suqiangkeji.cn/", ConfigurationName="WXPay.WebService1Soap")]
    public interface WebService1Soap {
        
        // CODEGEN: 命名空间 https://www.suqiangkeji.cn/ 的元素名称 HelloWorldResult 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.suqiangkeji.cn/HelloWorld", ReplyAction="*")]
        autosell_center.WXPay.HelloWorldResponse HelloWorld(autosell_center.WXPay.HelloWorldRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.suqiangkeji.cn/HelloWorld", ReplyAction="*")]
        System.Threading.Tasks.Task<autosell_center.WXPay.HelloWorldResponse> HelloWorldAsync(autosell_center.WXPay.HelloWorldRequest request);
        
        // CODEGEN: 命名空间 https://www.suqiangkeji.cn/ 的元素名称 channel 以后生成的消息协定未标记为 nillable
        [System.ServiceModel.OperationContractAttribute(Action="https://www.suqiangkeji.cn/CreateErWeiMa", ReplyAction="*")]
        autosell_center.WXPay.CreateErWeiMaResponse CreateErWeiMa(autosell_center.WXPay.CreateErWeiMaRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="https://www.suqiangkeji.cn/CreateErWeiMa", ReplyAction="*")]
        System.Threading.Tasks.Task<autosell_center.WXPay.CreateErWeiMaResponse> CreateErWeiMaAsync(autosell_center.WXPay.CreateErWeiMaRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorld", Namespace="https://www.suqiangkeji.cn/", Order=0)]
        public autosell_center.WXPay.HelloWorldRequestBody Body;
        
        public HelloWorldRequest() {
        }
        
        public HelloWorldRequest(autosell_center.WXPay.HelloWorldRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute()]
    public partial class HelloWorldRequestBody {
        
        public HelloWorldRequestBody() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class HelloWorldResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="HelloWorldResponse", Namespace="https://www.suqiangkeji.cn/", Order=0)]
        public autosell_center.WXPay.HelloWorldResponseBody Body;
        
        public HelloWorldResponse() {
        }
        
        public HelloWorldResponse(autosell_center.WXPay.HelloWorldResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.suqiangkeji.cn/")]
    public partial class HelloWorldResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string HelloWorldResult;
        
        public HelloWorldResponseBody() {
        }
        
        public HelloWorldResponseBody(string HelloWorldResult) {
            this.HelloWorldResult = HelloWorldResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CreateErWeiMaRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CreateErWeiMa", Namespace="https://www.suqiangkeji.cn/", Order=0)]
        public autosell_center.WXPay.CreateErWeiMaRequestBody Body;
        
        public CreateErWeiMaRequest() {
        }
        
        public CreateErWeiMaRequest(autosell_center.WXPay.CreateErWeiMaRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.suqiangkeji.cn/")]
    public partial class CreateErWeiMaRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string channel;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string mch_id;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string title;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string total_fee;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string client_ip;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=5)]
        public string url;
        
        public CreateErWeiMaRequestBody() {
        }
        
        public CreateErWeiMaRequestBody(string channel, string mch_id, string title, string total_fee, string client_ip, string url) {
            this.channel = channel;
            this.mch_id = mch_id;
            this.title = title;
            this.total_fee = total_fee;
            this.client_ip = client_ip;
            this.url = url;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class CreateErWeiMaResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="CreateErWeiMaResponse", Namespace="https://www.suqiangkeji.cn/", Order=0)]
        public autosell_center.WXPay.CreateErWeiMaResponseBody Body;
        
        public CreateErWeiMaResponse() {
        }
        
        public CreateErWeiMaResponse(autosell_center.WXPay.CreateErWeiMaResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="https://www.suqiangkeji.cn/")]
    public partial class CreateErWeiMaResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public string CreateErWeiMaResult;
        
        public CreateErWeiMaResponseBody() {
        }
        
        public CreateErWeiMaResponseBody(string CreateErWeiMaResult) {
            this.CreateErWeiMaResult = CreateErWeiMaResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WebService1SoapChannel : autosell_center.WXPay.WebService1Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WebService1SoapClient : System.ServiceModel.ClientBase<autosell_center.WXPay.WebService1Soap>, autosell_center.WXPay.WebService1Soap {
        
        public WebService1SoapClient() {
        }
        
        public WebService1SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WebService1SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebService1SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WebService1SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        autosell_center.WXPay.HelloWorldResponse autosell_center.WXPay.WebService1Soap.HelloWorld(autosell_center.WXPay.HelloWorldRequest request) {
            return base.Channel.HelloWorld(request);
        }
        
        public string HelloWorld() {
            autosell_center.WXPay.HelloWorldRequest inValue = new autosell_center.WXPay.HelloWorldRequest();
            inValue.Body = new autosell_center.WXPay.HelloWorldRequestBody();
            autosell_center.WXPay.HelloWorldResponse retVal = ((autosell_center.WXPay.WebService1Soap)(this)).HelloWorld(inValue);
            return retVal.Body.HelloWorldResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<autosell_center.WXPay.HelloWorldResponse> autosell_center.WXPay.WebService1Soap.HelloWorldAsync(autosell_center.WXPay.HelloWorldRequest request) {
            return base.Channel.HelloWorldAsync(request);
        }
        
        public System.Threading.Tasks.Task<autosell_center.WXPay.HelloWorldResponse> HelloWorldAsync() {
            autosell_center.WXPay.HelloWorldRequest inValue = new autosell_center.WXPay.HelloWorldRequest();
            inValue.Body = new autosell_center.WXPay.HelloWorldRequestBody();
            return ((autosell_center.WXPay.WebService1Soap)(this)).HelloWorldAsync(inValue);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        autosell_center.WXPay.CreateErWeiMaResponse autosell_center.WXPay.WebService1Soap.CreateErWeiMa(autosell_center.WXPay.CreateErWeiMaRequest request) {
            return base.Channel.CreateErWeiMa(request);
        }
        
        public string CreateErWeiMa(string channel, string mch_id, string title, string total_fee, string client_ip, string url) {
            autosell_center.WXPay.CreateErWeiMaRequest inValue = new autosell_center.WXPay.CreateErWeiMaRequest();
            inValue.Body = new autosell_center.WXPay.CreateErWeiMaRequestBody();
            inValue.Body.channel = channel;
            inValue.Body.mch_id = mch_id;
            inValue.Body.title = title;
            inValue.Body.total_fee = total_fee;
            inValue.Body.client_ip = client_ip;
            inValue.Body.url = url;
            autosell_center.WXPay.CreateErWeiMaResponse retVal = ((autosell_center.WXPay.WebService1Soap)(this)).CreateErWeiMa(inValue);
            return retVal.Body.CreateErWeiMaResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<autosell_center.WXPay.CreateErWeiMaResponse> autosell_center.WXPay.WebService1Soap.CreateErWeiMaAsync(autosell_center.WXPay.CreateErWeiMaRequest request) {
            return base.Channel.CreateErWeiMaAsync(request);
        }
        
        public System.Threading.Tasks.Task<autosell_center.WXPay.CreateErWeiMaResponse> CreateErWeiMaAsync(string channel, string mch_id, string title, string total_fee, string client_ip, string url) {
            autosell_center.WXPay.CreateErWeiMaRequest inValue = new autosell_center.WXPay.CreateErWeiMaRequest();
            inValue.Body = new autosell_center.WXPay.CreateErWeiMaRequestBody();
            inValue.Body.channel = channel;
            inValue.Body.mch_id = mch_id;
            inValue.Body.title = title;
            inValue.Body.total_fee = total_fee;
            inValue.Body.client_ip = client_ip;
            inValue.Body.url = url;
            return ((autosell_center.WXPay.WebService1Soap)(this)).CreateErWeiMaAsync(inValue);
        }
    }
}
