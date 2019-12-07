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
        	//�˴�����û���õ����õ�����������������
        	Util.debuglog("�յ����¹㲥11111","PACKAGE_REPLACED");
        	Toast.makeText(context, "��װ�˳����", Toast.LENGTH_LONG).show();
        	Log.i("7777", "123456");
        	String uname = Util.getSharePer(context, "mechineBH");
			String pwd = Util.getSharePer(context, "pwd");
			String mechineID=Util.getSharePer(context, "mechineID");
			String companyID=Util.getSharePer(context, "companyID");
			if(uname!=null&&pwd!=null&&mechineID!=null&&companyID!=null){
				Util.debuglog("�յ����¹㲥!=null","PACKAGE_REPLACED");
				 Intent intent2 = new Intent(context, IndexActivity.class);
		            intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		            context.startActivity(intent2);
			}
			else{
				Util.debuglog("�յ����¹㲥==null","PACKAGE_REPLACED");
				 Intent intent2 = new Intent(context, LoginActivity.class);
		            intent2.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
		            context.startActivity(intent2);
			}
           
        }
      //���հ�װ�㲥
//        if (intent.getAction().equals("android.intent.action.PACKAGE_ADDED")){
//            String packageName = intent.getDataString();
//        }
      //����ж�ع㲥
//        if (intent.getAction().equals("android.intent.action.PACKAGE_REMOVED")){
//            String packageName = intent.getDataString();
//        }
	}
}
