package com.bjw.ComAssistant;

import java.io.File;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;
import android.webkit.JavascriptInterface;
import android.webkit.JsResult;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;

public class WebViewActivity extends Activity {
	private WebView webView;
	//private final String url = "http://192.168.2.143:8088/web/login.html";
	private final String url="file:///android_asset/login.html";
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.webview);
		webView = (WebView) findViewById(R.id.webview);
		WebSettings settings= webView.getSettings();
		settings.setJavaScriptEnabled(true);//��ʾwebview ����ִ�з������˽��մ���
		//����webview���ļ��Ĳ���
		settings.setAllowUniversalAccessFromFileURLs(true);
		settings.setAllowFileAccess(true);
		settings.setAllowFileAccessFromFileURLs(true);
		settings.setDefaultTextEncodingName("utf-8");
		webView.setWebViewClient(new WebViewClient() {
			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				view.loadUrl(url);
				return true;
			}
		});
		webView.setWebChromeClient(new WebChromeClient(){
			@Override
			public void onProgressChanged(WebView view, int newProgress) {
				if(newProgress==100){
			        //ҳ��������ִ�еĲ���
			        String path= "file:///"+Environment.getExternalStorageDirectory()+ File.separator+"asm"+File.separator+"lbt2.jpg";
			        String action="javascript:aa('"+path+"')";
			        runWebView(action);
			      }
				super.onProgressChanged(view, newProgress);
			}
			@Override
			public boolean onJsAlert(WebView view, String url, String message,
					final JsResult result) {
				AlertDialog .Builder builder=new AlertDialog.Builder(WebViewActivity.this);
				builder.setTitle("��ʾ");
				builder.setMessage(message);
				builder.setPositiveButton("OK", new OnClickListener() {
					
					@Override
					public void onClick(DialogInterface dialog, int which) {
						result.confirm();//��ʾȷ���û���ѡ��
					}
				});
				builder.create().show();
				return true;
			}
		});
		webView.addJavascriptInterface(new JsObject(), "JsObject");
		webView.loadUrl(url);
	}
	
	public class JsObject{
		@JavascriptInterface
		public String getMessage(String name,String pwd)
		{
			//��ʾ�ӷ�������html���ݵ�ֵ
			System.out.println("-----client->>"+name+":"+pwd);
			return name+":"+pwd;
		}
	}
	private void runWebView(final String url){
	    runOnUiThread(new Runnable() {
	      @Override
	      public void run() {
	        webView.loadUrl(url);
	      }
	    });
	  }
}
