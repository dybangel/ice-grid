package com.bjw.ComAssistant;

import com.sq.util.Util;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;
import android.widget.Toast;
public class BCRUpgradeApk extends BroadcastReceiver {
	@Override
	public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals("android.intent.action.PACKAGE_REPLACED")){
        	//此处好像没有用到，用的命令解决的重启问题
        	Util.debuglog("收到更新广播11111","PACKAGE_REPLACED");
        	Toast.makeText(context, "安装了程序包", Toast.LENGTH_LONG).show();
        	Log.i("7777", "123456");
        	String uname = Util.getSharePer(context, "mechineBH");
			String pwd = Util.getSharePer(context, "pwd");
			String mechineID=Util.getSharePer(context, "mechineID");
			String companyID=Util.getSharePer(context, "companyID");
			if(uname!=null&&pwd!=null&&mechineID!=null&&companyID!=null){
				Util.debuglog("收到更新广播!=null","PACKAGE_REPLACED");
				 Intent intent2 = new Intent(context, IndexActivity.class);
		            intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		            context.startActivity(intent2);
			}
			else{
				Util.debuglog("收到更新广播==null","PACKAGE_REPLACED");
				 Intent intent2 = new Intent(context, LoginActivity.class);
		            intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		            context.startActivity(intent2);
			}
           
        }
      //接收安装广播
//        if (intent.getAction().equals("android.intent.action.PACKAGE_ADDED")){
//            String packageName = intent.getDataString();
//        }
      //接收卸载广播
//        if (intent.getAction().equals("android.intent.action.PACKAGE_REMOVED")){
//            String packageName = intent.getDataString();
//        }
	}
}
