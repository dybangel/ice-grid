package com.bjw.ComAssistant;

import com.baidu.location.*;
import com.sq.util.Config;
import com.sq.util.Util;

import android.app.Application;
import android.content.Intent;
import android.util.Log;
import android.widget.TextView;
import android.os.Process;
import android.os.Vibrator;

public class Location extends Application {

	public LocationClient mLocationClient = null;
	private String mData;
	public MyLocationListenner myListener = new MyLocationListenner();
	public String mTv;
	public NotifyLister mNotifyer = null;
	public Vibrator mVibrator01;

	@Override
	public void onCreate() {
		CrashHandler handler = CrashHandler.getInstance();
		handler.init(getApplicationContext()); // 在Appliction里面设置我们的异常处理器为UncaughtExceptionHandler处理器
		mLocationClient = new LocationClient(this);
		mLocationClient.registerLocationListener(myListener);
		super.onCreate();
	}

	/**
	 * 显示字符串
	 * 
	 * @param str
	 */
	public void logMsg(String str) {
		try {
			mData = str;
			if (mTv != null)
				mTv = mData;
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	/**
	 * 监听函数，又新位置的时候，格式化成字符串，输出到屏幕中
	 */
	public class MyLocationListenner implements BDLocationListener {
		@Override
		public void onReceiveLocation(BDLocation location) {
			if (location == null)
				return;
			
			StringBuffer sb = new StringBuffer(256);
			sb.append("time : ");
			sb.append(location.getTime());
			sb.append("\nerror code : ");
			sb.append(location.getLocType());
			sb.append("\nlatitude : ");
			sb.append(location.getLatitude());
			sb.append("\nlontitude : ");
			sb.append(location.getLongitude());
			sb.append("\nradius : ");
			sb.append(location.getRadius());
			if (location.getLocType() == BDLocation.TypeGpsLocation) {
				sb.append("\nspeed : ");
				sb.append(location.getSpeed());
				sb.append("\nsatellite : ");
				sb.append(location.getSatelliteNumber());
			} else if (location.getLocType() == BDLocation.TypeNetWorkLocation) {
				sb.append("\n省：");
				sb.append(location.getProvince());
				sb.append("\n市：");
				sb.append(location.getCity());
				sb.append("\n区/县：");
				sb.append(location.getDistrict());
				sb.append("\naddr : ");
				sb.append(location.getAddrStr());
				

			}
			sb.append("\nsdk version : ");
			sb.append(mLocationClient.getVersion());
			sb.append("\nisCellChangeFlag : ");
			sb.append(location.isCellChangeFlag());
			logMsg(sb.toString());
			
			String msg="{\"mechineID\":\""+Util.getSharePer(getApplicationContext(), "mechineID")+"\",\"Latitude\":\""+location.getLatitude()+"\","
					+ "\"Longitude\":\""+location.getLongitude()+"\",\"Province\":\""+location.getProvince()+"\",\"City\":\""+location.getCity()+"\","
							+ "\"District\":\""+location.getDistrict()+"\",\"Address\":\""+location.getAddrStr()+"\"}";
			final Intent intent = new Intent();
			intent.setAction(IndexActivity.ACTION_UPDATEUI);
			intent.putExtra("msg", msg);
			sendBroadcast(intent);
			Log.i("6666", "msg="+msg);
		}

		public void onReceivePoi(BDLocation poiLocation) {
			if (poiLocation == null) {
				return;
			}
			StringBuffer sb = new StringBuffer(256);
			sb.append("Poi time : ");
			sb.append(poiLocation.getTime());
			sb.append("\nerror code : ");
			sb.append(poiLocation.getLocType());
			sb.append("\nlatitude : ");
			sb.append(poiLocation.getLatitude());
			sb.append("\nlontitude : ");
			sb.append(poiLocation.getLongitude());
			sb.append("\nradius : ");
			sb.append(poiLocation.getRadius());
			if (poiLocation.getLocType() == BDLocation.TypeNetWorkLocation) {
				sb.append("\naddr : ");
				sb.append(poiLocation.getAddrStr());

			}
			if (poiLocation.hasPoi()) {
				sb.append("\nPoi:");
				sb.append(poiLocation.getPoi());
			} else {
				sb.append("noPoi information");
			}
			logMsg(sb.toString());
			final Intent intent = new Intent();
			intent.setAction(IndexActivity.ACTION_UPDATEUI);
			intent.putExtra("location", poiLocation.getAddrStr());
			intent.putExtra("latitude", poiLocation.getLatitude());
			intent.putExtra("lontitude", poiLocation.getLongitude());
			intent.putExtra("Province", poiLocation.getProvince());
			intent.putExtra("City", poiLocation.getCity());
			intent.putExtra("District", poiLocation.getDistrict());
			sendBroadcast(intent);
		}
	}

	public class NotifyLister extends BDNotifyListener {
		public void onNotify(BDLocation mlocation, float distance) {
			mVibrator01.vibrate(1000);
		}
	}
}