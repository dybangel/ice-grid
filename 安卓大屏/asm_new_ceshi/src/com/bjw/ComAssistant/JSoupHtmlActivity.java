package com.bjw.ComAssistant;

import android.app.Activity;
import android.graphics.Bitmap;
import android.os.Bundle;
import android.util.Log;
import android.webkit.JavascriptInterface;
import android.webkit.WebView;
import android.webkit.WebViewClient;

import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;
public class JSoupHtmlActivity extends Activity {
	private static final String DEFAULT_URL = "file:///android_asset/JsoupParHtml.html";
	WebView webView;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_jsoup_html);
		webView = (WebView) findViewById(R.id.webView);
		initData();
	}
	private void initData() {
		// 下面三行设置主要是为了待webView成功加载html网页之后，我们能够通过webView获取到具体的html字符串
		webView.getSettings().setJavaScriptEnabled(true);
		webView.addJavascriptInterface(new InJavaScriptLocalObj(), "local_obj");
		webView.setWebViewClient(new WebViewClient() {
			@Override
			public void onPageStarted(WebView view, String url, Bitmap favicon) {
				// TODO Auto-generated method stub
				super.onPageStarted(view, url, favicon);
			}
			 
			@Override
			public boolean shouldOverrideUrlLoading(WebView view, String url) {
				view.loadUrl(url);
				return true;
			}
			@Override
			public void onPageFinished(WebView view, String url) {
				super.onPageFinished(view, url);
				view.loadUrl("javascript:window.local_obj.showSource('<head>'+"
						+ "document.getElementsByTagName('html')[0].innerHTML+'</head>');");
			}
			@Override
			public void onReceivedError(WebView view, int errorCode,
					String description, String failingUrl) {
				super.onReceivedError(view, errorCode, description, failingUrl);
			}

		});
		webView.loadUrl(DEFAULT_URL);
	}

	final class InJavaScriptLocalObj {
		@JavascriptInterface
		public void showSource(String html) {
			refreshHtmlContent(html);
		}
	}

	private void refreshHtmlContent(final String html) {
		Log.i("网页内容", html);
		webView.postDelayed(new Runnable() {
			@Override
			public void run() {
				// 解析html字符串为对象
				Document document = Jsoup.parse(html);
				// 通过类名获取到一组Elements，获取一组中第一个element并设置其html
				Elements elements = document.getElementsByClass("loadDesc");
				elements.get(0).html("<p>加载完成</p>");
				// 通过ID获取到element并设置其src属性
				Element element = document.getElementById("imageView");
				element.attr("src", "file:///android_asset/dragon.jpg");
				// 通过类名获取到一组Elements，获取一组中第一个element并设置其文本
				elements = document.select("p.hint"); //p标签 下的hint样式
				elements.get(0).text("您好，我是龙猫军团！");
				// 获取进行处理之后的字符串并重新加载
				String body = document.toString();
				Log.i("2222", "body="+body);
				webView.loadDataWithBaseURL(null, body, "text/html", "utf-8",null);
			}
		}, 5000);
	}
}
