package com.bjw.ComAssistant;

import java.io.IOException;
import java.util.jar.Attributes.Name;

import org.apache.http.client.HttpResponseException;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.webkit.JavascriptInterface;
import android.webkit.JsResult;
import android.webkit.WebChromeClient;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import com.sq.util.CCGlobalProvider;
import com.sq.util.Config;
import com.sq.util.Util;

public class LoginActivity extends Activity {

	private WebView loginWebView;
	private final String url = "file:///android_asset/login.html";

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		// TODO Auto-generated method stub
		super.onCreate(savedInstanceState);
		setContentView(R.layout.login);

		loginWebView = (WebView) findViewById(R.id.loginweb);
		WebSettings settings = loginWebView.getSettings();
		settings.setJavaScriptEnabled(true);// 表示webview 可以执行服务器端接收代码
		// 允许webview对文件的操作
		settings.setAllowUniversalAccessFromFileURLs(true);
		settings.setAllowFileAccess(true);
		settings.setAllowFileAccessFromFileURLs(true);
		settings.setDefaultTextEncodingName("utf-8");

		loginWebView.setWebChromeClient(new WebChromeClient() {
			@Override
			public boolean onJsAlert(WebView view, String url, String message, final JsResult result) {
				AlertDialog.Builder builder = new AlertDialog.Builder(LoginActivity.this);
				builder.setTitle("提示");
				builder.setMessage(message);
				builder.setPositiveButton("OK", new OnClickListener() {

					@Override
					public void onClick(DialogInterface dialog, int which) {
						result.confirm();// 表示确认用户的选择
					}
				});
				builder.create().show();
				return true;
			}
		});
		loginWebView.setWebViewClient(new WebViewClient() {
			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				view.loadUrl(url);
				return true;
			}
		});
		loginWebView.setOnTouchListener(new View.OnTouchListener() {
			
			@Override
			public boolean onTouch(View v, MotionEvent event) {
				switch (event.getAction()) { 
	               case MotionEvent.ACTION_DOWN: 
	               case MotionEvent.ACTION_UP: 
	                   if (!v.hasFocus()) { 
	                       v.requestFocus(); 
	                   } 
	                   break; 
	           } 
	           return false; 
	        }
		});
		loginWebView.addJavascriptInterface(new JsObject(), "JsObject");
		loginWebView.loadUrl(url);

		// 创建文件夹
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/");
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/data/");
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/image/");
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/video/");
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/video/hvideo/");
		Util.isExist(Environment.getExternalStorageDirectory() + "/asm/video/vvideo/");
		// 判断用户名和密码是否存在 并验证是否正确 正确自动登录
		new Thread(new Runnable() {
			@Override
			public void run() {
				try {
					String uname = Util.getSharePer(LoginActivity.this, "mechineBH");
					String pwd = Util.getSharePer(LoginActivity.this, "pwd");
					Login(uname, pwd);
				} catch (Exception e) {

				}

			}
		}).start();
	}
	public String Login(String name, String pwd) {
		Util.debuglog("Login"+name+pwd,"qd");
		// 表示从服务器端html传递的值
		String MethodName = "Login";
		String soapAction = Config.NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(Config.NameSpace, MethodName);
		request.addProperty("BH", name);
		request.addProperty("pwd", pwd);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER12);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(Config.url);
		try {
			ht.call(soapAction, envelope);
			Log.i("8888","1name："+name+"1pwd："+pwd);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				Log.i("8888","result："+result);
				//
			}
		} catch (Exception e) {
			Log.i("8888","Exception："+e.toString());
			e.printStackTrace();
			return "{\"code\":\"500\",\"msg\":\"系统异常\"}";
		}
		if ("1".equals(result)) {
			Log.i("8888","账号密码错误：");
			return "{\"code\":\"500\",\"msg\":\"账号密码错误\"}";
		} else {
			
			// 解析result
			String mechineID = "";
			try {
				JSONArray jsonArray = new JSONArray(result);
				JSONObject jsonObject = jsonArray.getJSONObject(0);
				Util.setSharePer(LoginActivity.this, "mechineBH", jsonObject.getString("bh"));
				Util.setSharePer(LoginActivity.this, "mechineID", jsonObject.getString("id"));
				Util.setSharePer(LoginActivity.this, "pwd", pwd);
				Util.setSharePer(LoginActivity.this, "companyID", jsonObject.getString("companyID"));
				Util.setSharePer(LoginActivity.this, "socketUrl", jsonObject.getString("socketUrl"));
				mechineID = jsonObject.getString("id");
			} catch (JSONException e) {
				e.printStackTrace();
			}
			Log.i("8888","intent：");
			Intent intent = new Intent();
			intent.setClass(LoginActivity.this, IndexActivity.class);// 跳转到IndexActivity
			intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
			startActivity(intent);
			//finish();
			return "{\"code\":\"200\",\"msg\":\"" + mechineID + "\"}";
		}
	}
	public class JsObject {
		@JavascriptInterface
		public String login(String name, String pwd) {
			return Login(name, pwd);
		}
	}
	public void destroyWebView() {
		if (loginWebView != null) {
			loginWebView.clearHistory();
			loginWebView.clearCache(true);
			loginWebView.loadUrl("about:blank"); // clearView() should be changed to loadUrl("about:blank"), since
													// clearView() is deprecated now
			loginWebView.freeMemory();
			loginWebView.pauseTimers();
			loginWebView = null; // Note that mWebView.destroy() and mWebView = null do the exact same thing
		}
	}
	@Override
	protected void onDestroy() {
		// TODO Auto-generated method stub
		super.onDestroy();
		destroyWebView();
	}

}
