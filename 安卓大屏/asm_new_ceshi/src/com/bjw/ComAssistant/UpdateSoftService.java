package com.bjw.ComAssistant;

import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.location.LocationManager;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.os.Vibrator;
import android.util.Log;

import java.text.SimpleDateFormat;
import java.util.Date;

import org.json.JSONArray;
import org.json.JSONObject;

import com.baidu.location.LocationClient;
import com.baidu.location.LocationClientOption;
import com.sq.util.Config;
import com.sq.util.Util;

public class UpdateSoftService extends Service {

	private UpdateManager mUpdateManager;// �������
	private Vibrator mVibrator01 = null;
	private LocationClient mLocClient;
	private LocationManager mLocationManager;
	private String mtv;

	/**
	 * �󶨷���ʱ�Ż���� ����Ҫʵ�ֵķ���
	 * 
	 * @param intent
	 * @return
	 */
	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	/**
	 * �״δ�������ʱ��ϵͳ�����ô˷�����ִ��һ�������ó����ڵ��� onStartCommand() �� onBind() ֮ǰ����
	 * ��������������У��򲻻���ô˷������÷���ֻ������һ��
	 */
	@Override
	public void onCreate() {
		try {
			mLocationManager = (LocationManager) getApplicationContext()
					.getSystemService(Context.LOCATION_SERVICE);
			mLocClient = ((Location) getApplication()).mLocationClient;
			((Location) getApplication()).mTv = mtv;
			mVibrator01 = (Vibrator) getApplication().getSystemService(
					Service.VIBRATOR_SERVICE);
			((Location) getApplication()).mVibrator01 = mVibrator01;
			setLocationOption();
			mLocClient.start();

//			Thread td1 = new Thread(updateRunnable, "updateRunnable");// �������
//			td1.start();

			super.onCreate();
		} catch (Exception e) {
		}
	}

	public void showToast(String str) {
		Intent intent = new Intent();
		intent.putExtra("toast", "" + str);
		intent.setAction("com.tools.ui.toast");
		sendBroadcast(intent);
	}

	/**
	 * ÿ��ͨ��startService()��������Serviceʱ���ᱻ�ص���
	 * 
	 * @param intent
	 * @param flags
	 * @param startId
	 * @return
	 */
	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		mLocClient = ((Location) getApplication()).mLocationClient;
		((Location) getApplication()).mTv = mtv;
		mVibrator01 = (Vibrator) getApplication().getSystemService(
				Service.VIBRATOR_SERVICE);
		((Location) getApplication()).mVibrator01 = mVibrator01;
		setLocationOption();
		mLocClient.requestLocation();

		// mHanlder.sendEmptyMessage(1);

		return super.onStartCommand(intent, flags, startId);
	}

	/**
	 * ��������ʱ�Ļص�
	 */
	@Override
	public void onDestroy() {
		super.onDestroy();
	}

	private Handler mHanlder = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case 1:
				// app����
				mUpdateManager = new UpdateManager(getApplicationContext());
				if (mUpdateManager.getServerVer(getApplicationContext())) {

				}
				break;

			default:
				break;
			}
			super.handleMessage(msg);
		}
	};

	private Runnable updateRunnable = new Runnable() {
		@Override
		public void run() {
			/**
			 * �˴�ִ������ �������Ƿ��и���
			 * */
			while (true) {
				try {
					Thread.sleep(1000 * 60);
					String mechineInfo = Util.getSharePer(getApplicationContext(), "mechineInfo");
					if (!"".equals(mechineInfo) && mechineInfo != null) {
						Log.i("3333", "mechineInfo="+mechineInfo);
						try {
							JSONArray jsonArray = new JSONArray(mechineInfo);
							JSONObject object = jsonArray.getJSONObject(0);
							String p2 = object.getString("p2");
							SimpleDateFormat sdfDateFormat = new SimpleDateFormat("HH:mm");
							String time = sdfDateFormat.format(new Date());
							if (time.equals(p2)) {
								mHanlder.sendEmptyMessage(1);
							}
						} catch (Exception e) {

						}
					}
				} catch (Exception e1) {
					e1.printStackTrace();
				}
			}
		}
	};

	// ������ز���
	private void setLocationOption() {
		LocationClientOption option = new LocationClientOption();
		option.setOpenGps(true); // ��gps
		option.setCoorType("bd09ll"); // ������������
		option.setServiceName("com.baidu.location.service_v2.9");
		option.setPoiExtraInfo(true);
		option.setAddrType("all");
		option.setScanSpan(3000);
		if (mLocationManager.isProviderEnabled(LocationManager.GPS_PROVIDER)) {
			option.setPriority(LocationClientOption.GpsFirst); // ������������
		} else {
			option.setPriority(LocationClientOption.NetWorkFirst); // ������������
		}
		option.setPoiNumber(10);
		option.disableCache(true);
		mLocClient.setLocOption(option);
	}

}
