package com.bjw.ComAssistant;
import com.sq.util.Config;
import com.sq.util.Util;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
public class BootBroadcastReceiver extends BroadcastReceiver {
	static final String ACTION = "android.intent.action.BOOT_COMPLETED";
	@Override
	public void onReceive(Context context, Intent intent) {
		// 开机启动
		if (intent.getAction().equals("android.intent.action.BOOT_COMPLETED")) {
			String uname = Util.getSharePer(context, "mechineBH");
			String pwd = Util.getSharePer(context, "pwd");
			String mechineID=Util.getSharePer(context, "mechineID");
			String companyID=Util.getSharePer(context, "companyID");
			if(uname!=null&&pwd!=null&&mechineID!=null&&companyID!=null){
				
				Util.debuglog("收到启动广播IndexActivity","qd");
				Intent intentMainActivity = new Intent(context, IndexActivity.class);
				intentMainActivity.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
				context.startActivity(intentMainActivity);
			}
			else{
				
				Util.debuglog("收到启动广播LoginActivity","qd");
				Intent intentMainActivity = new Intent(context, LoginActivity.class);
				intentMainActivity.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
				context.startActivity(intentMainActivity);
			}
			
		}
		// TODO Auto-generated method stub
		if (intent.getAction().equals("android.intent.action.PACKAGE_REPLACED")) {
			Util.debuglog("收到更新广播IndexActivity","PACKAGE_REPLACED");
			// Toast.makeText(context,"升级了一个安装包，重新启动此程序", Toast.LENGTH_SHORT).show();
			String packageName = intent.getDataString();
			Log.e("更新成功", "" + packageName);
//			Intent intent2 = new Intent(context, IndexActivity.class);
//			intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
//			context.startActivity(intent2);
		}
		 //接收安装广播
		if (intent.getAction().equals("android.intent.action.PACKAGE_ADDED")) {
			Log.i("8888", "BootBBBBPACKAGE_ADDED");
			Util.debuglog("收到安装广播PACKAGE_ADDED","PACKAGE_REPLACED");
			String packageName = intent.getDataString();
			System.out.println("安装了:" + packageName + "包名的程序");
		}
		// 接收卸载广播
		if (intent.getAction().equals("android.intent.action.PACKAGE_REMOVED")) {
			String packageName = intent.getDataString();
			System.out.println("卸载了:" + packageName + "包名的程序");
		}
	}
}
