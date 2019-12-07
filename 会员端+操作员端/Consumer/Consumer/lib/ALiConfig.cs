using System;
using System.Collections.Generic;
using System.Web;

namespace WxPayAPI
{
    /**
    * 	配置账号信息
    */
    public class ALiConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = "2016101200668456";
        public const string privateKey = "MIIEpAIBAAKCAQEA1bA2k31E7vOqqJm1pmwRhJ11VKTXPE/fEYrVhMyaBg3fiO3fpGKNIgJ1AYaAmH7py4GH648dVrcJ39pmlmP1OojpmN7HBL9wuNuB96TAGCGx1iC0ppdiuvhjBxSMP32Ns6vKYX6ajL02pYNbblgyEssmh+dOfJTh9NC9pstVPOEezig734nrb2t//V9VaSAMI3waX4cL9vftDeg/npBFvSuZG4LU03RRD1FX8uMf9awz1tEULXIca8TZ3EOD4HWu2+IsH+swnxtthGPYf8irvxwo89jc8O/c3eR9oDDHtkmsd3EvdYCyFkvBLglva5jIwR0+Aav3qPDCglq1B/9V3QIDAQABAoIBAAYTD0ocCoScaqKGVBKaCdlyPG6ejPvK7XVrM+ylgE9hv5P95xieMJLh7P+RGkC7gtvlH78Df3sCkwZJCBeIWeVDFRjiZFfvNT4cB1LJ3SgSdSK4JUqDDSxxeScvX1lCsW+FF6iz16LekhocZNH7Mdk06zB709BYzx+Ne4atSwwC8PIkCwnqzWpVGov5PRGmzM1gZYskf6Zcls/rPCN5Am0jxMORZo+exE86w+MEP18mo6Rrkqb5gbgHH3GqPxY/QrQ9cwijklL6GyEy8yGtdmNjM0/lKO59EZNHI7f3FYoudh9UlGdcLFWW1DXVoMRfl4Bm4/gAChcaWtbqwc4ZkCECgYEA+BY9dM+DIP5BudSb07hewZvmkjoNlRt760rrQJLsvz/lQyCj4HD8LpHqA+7waFeRjoyAO9lKllbNr+/T4FlJqReyXbRwGLf37ihS4xaPo1CSU4pu1loo9W3rwr9paobRMxDbNfnzyhf9uoqDb+/o2z0lFeKickEirTkrAJjHG8UCgYEA3IEXRk/x/upKOFawZn9xHfvO2Avo70AJp9Lr8v5KFWDNs1/YwNocWkeqO3LSXzTYKpKQu2WVfwAZuhlGcvQXsTBPPHmwRvXIYM/k8O14O9hD7ouQqZvceBXeNbXu3pd97dFlgR4Hy5TjAkmtfX2Pyla06TOCtO5FXYcT2SSw+zkCgYBX6cmtczvMnU09RRJTXKp3gX+boebeR/cJ0mq7X1V2EHZ160MnbeJvvlVnP20CDMYP9coba0z7KZeKGTrD7eAkg7k5a4+tssOxKaj1wDD2dr7jy9KrMxgAoQtC4AHDNjM8HWURI5o4q3fGev9I04N47ZoOv1lBk8NuDywl8f3c4QKBgQDQV2CIXM0H0JLj/HSbw9o7PErJsycZt2XoHdA2PbH91QNGck84mFylqITdurWuox2DzAvYJDlGT+/++BaqUpt4HW8gctHtmhkD8CoewiESWlqFL3U17EA5bmAQW6AgzP59D5ZakudGoZyD8t1rbpHH2nkAxly/W0pK/m/wgUx/cQKBgQDdhYbPUJHuntZCYO2H3pDkDn/IcdpyakSvHayAsNtKocscUE1WpeCNTRjJQzutooiqqVp3WlRnAQHjRrj2UrHZUR/jKKOPAAiY+uQseB5ZDCy36lXqyHyGb3qmHDqgUBopx5RXJwOKPDjlImgNTfDvEG3n1fgAq1oJ3h1aTdUeSg==";
        public const string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEApaRfHCMN88nhrOyfLqL9wwXis2PcX0UgO1SnB20u2Vuz2zxsx4cgNYJIhAmT6pwDvDXsjrSzwvFcu9S6I75IKcSOQsPPLtUiIPn0ig8Jh0qpLJaS9AlGdz+yUGwilhvSdv5uum9q8NznZH0wbZEJ90A2MTKSmSLAJ4ysn2gyifXc+2DJK39/Jxg2sqIPYgSV5zqUD+CO1mL7qgS16VWlYDMUz4edWvJwqNUhSbYur8xKfFoyMFIVzy+f7bF6C+5UeIVaG97UFzr9ZXppMQBSXRcznpbhbxloklx3odg2p9B03eV/CR/WMdZCZBWpjf65rqieVMWoJ+aBtqIZoYuEQQIDAQAB";
        public const string serviceUrl = "https://openapi.alipaydev.com/gateway.do";


        public const string APPSECRET = "5fd73e115d4da4b21b685e28b70b71d8";
        

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "https://wx.bingoseller.com/pay/Notify.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }
}