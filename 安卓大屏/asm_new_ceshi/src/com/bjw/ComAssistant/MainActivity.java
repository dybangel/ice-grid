package com.bjw.ComAssistant;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.sq.dao.Dao;
import com.sq.util.Util;

public class MainActivity extends Activity {
	public EditText ldNo;
	public Button btnCH, btnTem,btnGo;
	public TextView lblInfo, temInfo, locationInfo;
	public static final String ACTION_UPDATEUI = "action.updateUI";
	public static final String PORT_ACTION = "action.port";
	public static final String DIALOG_ACTION = "action.dialog";
	public static final String LOADING_ACTION = "action.loading";
	private ProgressDialog progressDialog;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);
		ldNo = (EditText) findViewById(R.id.ldNO);
		btnCH = (Button) findViewById(R.id.btnCH);
		btnTem = (Button) findViewById(R.id.btnTem);
		btnGo=(Button)findViewById(R.id.btnGo);
		lblInfo = (TextView) findViewById(R.id.lblInfo);
		temInfo = (TextView) findViewById(R.id.temInfo);
		locationInfo = (TextView) findViewById(R.id.locationInfo);
		btnCH.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				String ldNO = ldNo.getText().toString();
				if (ldNO != null) {
					Intent intent = new Intent(MainActivity.this,
							PortService.class);
					intent.putExtra("is_ch", true);
					intent.putExtra("ldNO", ldNo.getText().toString());
					startService(intent);
				}
			}
		});
		btnTem.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
//				Intent intent = new Intent(MainActivity.this, PortService.class);
//				intent.putExtra("is_search_tem", true);
//				startService(intent);
				//生成订单信息
				new Thread(new Runnable() {
					@Override
					public void run() {
						// TODO Auto-generated method stub
						Dao dao=new Dao();
					}
				}).start();
			}
		});
		btnGo.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				 Intent intent=new Intent(MainActivity.this,WebViewActivity.class);
				 intent.setClass(MainActivity.this, WebViewActivity.class);
		         startActivity(intent);
			}
		});
		// 注册UI广播
		IntentFilter uiIntentFilter = new IntentFilter();
		uiIntentFilter.addAction("com.tools.ui.toast");
		uiIntentFilter.addAction(ACTION_UPDATEUI);
		uiIntentFilter.addAction(PORT_ACTION);
		registerReceiver(uiReceiver, uiIntentFilter);
		//启动服务
		Intent intent = new Intent(this, UpdateSoftService.class);
		startService(intent);
		Intent intent1 = new Intent(this, WebSocketService.class);
		startService(intent1);
		Intent intent2 = new Intent(this, PortService.class);
		startService(intent2);
		//executeSQL();
		
	}
	
	private BroadcastReceiver uiReceiver = new BroadcastReceiver() {
		@Override
		public void onReceive(final Context context, Intent intent) {
			String action = intent.getAction();
			if (action.equals("com.tools.ui.toast")) {
				Toast.makeText(MainActivity.this,"" + intent.getStringExtra("toast"), Toast.LENGTH_LONG).show();
			} else if (action.equals(DIALOG_ACTION)) {
				if (progressDialog == null) {
					hideProgressDialog();
				}
				// 弹出框
				AlertDialog alertDialog2 = new AlertDialog.Builder(
						MainActivity.this)
						.setTitle("温馨提示")
						.setMessage("有多个按钮")
						.setIcon(R.drawable.ic_launcher)
						.setPositiveButton("确定",
								new DialogInterface.OnClickListener() {// 添加"Yes"按钮
									@Override
									public void onClick(DialogInterface dialogInterface,int i) {

									}
								}).create();
				alertDialog2.show();
			} else if (action.equals(LOADING_ACTION)) {
				//加载框
				showProgressDialog("温馨提示", "正在出货，请稍后...");
			} else if (action.equals(ACTION_UPDATEUI)) {
				temInfo.setText("温度信息:"+ intent.getExtras().getInt("temperature") + ";版本信息:"+ Util.getVerName(MainActivity.this));
				locationInfo.setText("位置信息："+ intent.getExtras().getString("location"));
			} else if (action.equals(PORT_ACTION)) {
				if (lblInfo.getLineCount() >= 45) {
					lblInfo.setText("");
				}
				lblInfo.append(String.valueOf(intent.getExtras().getString("str"))+ "\r\n");
			}
		}
	};

	/*
	 * 提示加载
	 */
	public void showProgressDialog(String title, String message) {
		if (progressDialog == null) {
			progressDialog = ProgressDialog.show(MainActivity.this, title,
					message, true, false);
		} else if (progressDialog.isShowing()) {
			progressDialog.setTitle(title);
			progressDialog.setIcon(R.drawable.dialog_tip);
			progressDialog.setMessage(message);
		}
		progressDialog.show();
	}

	/*
	 * 隐藏提示加载
	 */
	public void hideProgressDialog() {
		if (progressDialog != null && progressDialog.isShowing()) {
			progressDialog.dismiss();
		}
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		unregisterReceiver(uiReceiver);
	}
	 
}
