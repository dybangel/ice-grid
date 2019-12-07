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
		// ��������������Ҫ��Ϊ�˴�webView�ɹ�����html��ҳ֮�������ܹ�ͨ��webView��ȡ�������html�ַ���
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
		Log.i("��ҳ����", html);
		webView.postDelayed(new Runnable() {
			@Override
			public void run() {
				// ����html�ַ���Ϊ����
				Document document = Jsoup.parse(html);
				// ͨ��������ȡ��һ��Elements����ȡһ���е�һ��element��������html
				Elements elements = document.getElementsByClass("loadDesc");
				elements.get(0).html("<p>�������</p>");
				// ͨ��ID��ȡ��element��������src����
				Element element = document.getElementById("imageView");
				element.attr("src", "file:///android_asset/dragon.jpg");
				// ͨ��������ȡ��һ��Elements����ȡһ���е�һ��element���������ı�
				elements = document.select("p.hint"); //p��ǩ �µ�hint��ʽ
				elements.get(0).text("���ã�������è���ţ�");
				// ��ȡ���д���֮����ַ��������¼���
				String body = document.toString();
				Log.i("2222", "body="+body);
				webView.loadDataWithBaseURL(null, body, "text/html", "utf-8",null);
			}
		}, 5000);
	}
}
