package com.sq.util;

import java.util.List;

import android.content.Context;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.telephony.CellInfo;
import android.telephony.CellInfoCdma;
import android.telephony.CellInfoGsm;
import android.telephony.CellInfoLte;
import android.telephony.CellInfoWcdma;
import android.telephony.CellSignalStrengthCdma;
import android.telephony.CellSignalStrengthGsm;
import android.telephony.CellSignalStrengthLte;
import android.telephony.CellSignalStrengthWcdma;
import android.telephony.TelephonyManager;
import android.util.Log;

public class NetUtils {
	public static final int NETWORK_NONE = 0; // û����������
	public static final int NETWORK_WIFI = 1; // wifi����
	public static final int NETWORK_2G = 2; // 2G
	public static final int NETWORK_3G = 3; // 3G
	public static final int NETWORK_4G = 4; // 4G
	public static final int NETWORK_MOBILE = 5; // �ֻ�����

	/**
	 * ��ȡ��Ӫ������
	 * 
	 * @param context
	 *            context
	 * @return int
	 */
	public static String getOperatorName(Context context) {
		/*
		 * getSimOperatorName()�Ϳ���ֱ�ӻ�ȡ����Ӫ�̵�����
		 * Ҳ����ʹ��IMSI��ȡ��getSimOperator()��Ȼ����ݷ���ֵ�жϣ�����"46000"Ϊ�ƶ�
		 * IMSI������ӣ�http://baike.baidu.com/item/imsi
		 */
		TelephonyManager telephonyManager = (TelephonyManager) context
				.getSystemService(Context.TELEPHONY_SERVICE);
		// getSimOperatorName�Ϳ���ֱ�ӻ�ȡ����Ӫ�̵�����
		return telephonyManager.getSimOperatorName();
	}

	/**
	 * ��ȡ��ǰ�������ӵ�����
	 * 
	 * @param context
	 *            context
	 * @return int
	 */
	public static int getNetworkState(Context context) {
		
		ConnectivityManager connManager = (ConnectivityManager) context
				.getSystemService(Context.CONNECTIVITY_SERVICE); // ��ȡ�������
		if (null == connManager) { // Ϊ������Ϊ������
			return NETWORK_NONE;
		}
		
		// ��ȡ�������ͣ����Ϊ�գ�����������
		NetworkInfo activeNetInfo = connManager.getActiveNetworkInfo();
		if (activeNetInfo == null || !activeNetInfo.isAvailable()) {
			return NETWORK_NONE;
		}
		// �ж��Ƿ�ΪWIFI
		NetworkInfo wifiInfo = connManager
				.getNetworkInfo(ConnectivityManager.TYPE_WIFI);
		if (null != wifiInfo) {
			
			NetworkInfo.State state = wifiInfo.getState();
			if (null != state) {
				if (state == NetworkInfo.State.CONNECTED
						|| state == NetworkInfo.State.CONNECTING) {
					return NETWORK_WIFI;
				}
			}
		}
		// ������WIFI����ȥ�ж���2G��3G��4G��
		TelephonyManager telephonyManager = (TelephonyManager) context
				.getSystemService(Context.TELEPHONY_SERVICE);
		int networkType = telephonyManager.getNetworkType();
		
		switch (networkType) {
		/*
		 * GPRS : 2G(2.5) General Packet Radia Service 114kbps EDGE : 2G(2.75G)
		 * Enhanced Data Rate for GSM Evolution 384kbps UMTS : 3G WCDMA ��ͨ3G
		 * Universal Mobile Telecommunication System ������3G�ƶ�ͨ�ż�����׼ CDMA : 2G ����
		 * Code Division Multiple Access ��ֶ�ַ EVDO_0 : 3G (EVDO ȫ�� CDMA2000
		 * 1xEV-DO) Evolution - Data Only (Data Optimized) 153.6kps - 2.4mbps
		 * ����3G EVDO_A : 3G 1.8mbps - 3.1mbps ����3G���ɣ�3.5G 1xRTT : 2G CDMA2000
		 * 1xRTT (RTT - ���ߵ紫�似��) 144kbps 2G�Ĺ���, HSDPA : 3.5G �������з������ 3.5G WCDMA
		 * High Speed Downlink Packet Access 14.4mbps HSUPA : 3.5G High Speed
		 * Uplink Packet Access ����������·������� 1.4 - 5.8 mbps HSPA : 3G
		 * (��HSDPA,HSUPA) High Speed Packet Access IDEN : 2G Integrated Dispatch
		 * Enhanced Networks ����������ǿ������ ������2G������ά���ٿƣ� EVDO_B : 3G EV-DO Rev.B
		 * 14.7Mbps ���� 3.5G LTE : 4G Long Term Evolution FDD-LTE �� TDD-LTE ,
		 * 3G���ɣ������� LTE Advanced ����4G EHRPD : 3G CDMA2000��LTE 4G���м���� Evolved
		 * High Rate Packet Data HRPD������ HSPAP : 3G HSPAP �� HSDPA ��Щ
		 */
		// 2G����
		case TelephonyManager.NETWORK_TYPE_GPRS:
		case TelephonyManager.NETWORK_TYPE_CDMA:
		case TelephonyManager.NETWORK_TYPE_EDGE:
		case TelephonyManager.NETWORK_TYPE_1xRTT:
		case TelephonyManager.NETWORK_TYPE_IDEN:
			return NETWORK_2G;
			// 3G����
		case TelephonyManager.NETWORK_TYPE_EVDO_A:
		case TelephonyManager.NETWORK_TYPE_UMTS:
		case TelephonyManager.NETWORK_TYPE_EVDO_0:
		case TelephonyManager.NETWORK_TYPE_HSDPA:
		case TelephonyManager.NETWORK_TYPE_HSUPA:
		case TelephonyManager.NETWORK_TYPE_HSPA:
		case TelephonyManager.NETWORK_TYPE_EVDO_B:
		case TelephonyManager.NETWORK_TYPE_EHRPD:
		case TelephonyManager.NETWORK_TYPE_HSPAP:
			return NETWORK_3G;
			// 4G����
		case TelephonyManager.NETWORK_TYPE_LTE:
			return NETWORK_4G;
		default:
			return NETWORK_MOBILE;
		}
	}

	/**
	 * �ж������Ƿ�����
	 * 
	 * @param context
	 *            context
	 * @return true/false
	 */
	public static boolean isNetConnected(Context context) {
		ConnectivityManager connectivity = (ConnectivityManager) context
				.getSystemService(Context.CONNECTIVITY_SERVICE);
		if (connectivity != null) {
			NetworkInfo info = connectivity.getActiveNetworkInfo();
			if (info != null && info.isConnected()) {
				if (info.getState() == NetworkInfo.State.CONNECTED) {
					return true;
				}
			}
		}
		return false;
	}

	/**
	 * �ж��Ƿ�wifi����
	 * 
	 * @param context
	 *            context
	 * @return true/false
	 */
	public static synchronized boolean isWifiConnected(Context context) {
		ConnectivityManager connectivityManager = (ConnectivityManager) context
				.getSystemService(Context.CONNECTIVITY_SERVICE);
		if (connectivityManager != null) {
			NetworkInfo networkInfo = connectivityManager
					.getActiveNetworkInfo();
			if (networkInfo != null) {
				int networkInfoType = networkInfo.getType();
				if (networkInfoType == ConnectivityManager.TYPE_WIFI
						|| networkInfoType == ConnectivityManager.TYPE_ETHERNET) {
					return networkInfo.isConnected();
				}
			}
		}
		return false;
	}

	public static int getLteLevel(int gsmSignalStrength) {
		if (gsmSignalStrength > -44) {
			return -1;

		} else if (gsmSignalStrength > -97) {
			return 4;

		} else if (gsmSignalStrength > -105) {
			return 3;

		} else if (gsmSignalStrength > -110) {
			return 2;

		} else if (gsmSignalStrength > -120) {
			return 1;

		} else if (gsmSignalStrength >= -140) {
			return 0;

		} else {

			return -1;
		}
	}

	public static int getNetworkWifiLevel(Context context) {
		if (!isWifiConnected(context)) {
			return 0;
		}

		WifiManager wifiManager = (WifiManager) context
				.getSystemService(Context.WIFI_SERVICE);

		WifiInfo wifiInfo = wifiManager.getConnectionInfo();
		// ����ź�ǿ��ֵ
		// if (Build.VERSION.SDK_INT >= Build.VERSION_CODES) {
		int level = wifiInfo.getRssi();
		// ���ݻ���źŵ�ǿ�ȷ�����Ϣ
		if (level <= 0 && level >= -50) {// ��ǿ
			System.out.println("level==========1===========" + level);
			return 4;
		} else if (level < -50 && level >= -70) {// ��ǿ
			System.out.println("level===========2==========" + level);
			return 3;
		} else if (level < -70 && level >= -80) {// ����
			System.out.println("level==========3===========" + level);
			return 2;
		} else if (level < -80 && level >= -100) {// ΢��
			System.out.println("level==========4===========" + level);
			return 1;
		} else {
			System.out.println("level==========5===========" + level);
			return 0;
		}
		// }

		// return 0;
	}

	public static int getMobileDbm(Context context) {
		int dbm = -1;
		TelephonyManager tm = (TelephonyManager) context
				.getSystemService(Context.TELEPHONY_SERVICE);
		List<CellInfo> cellInfoList;
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
			cellInfoList = tm.getAllCellInfo();
			if (null != cellInfoList) {
				for (CellInfo cellInfo : cellInfoList) {
					if (cellInfo instanceof CellInfoGsm) {
						CellSignalStrengthGsm cellSignalStrengthGsm = ((CellInfoGsm) cellInfo)
								.getCellSignalStrength();
						dbm = cellSignalStrengthGsm.getDbm();
					} else if (cellInfo instanceof CellInfoCdma) {
						CellSignalStrengthCdma cellSignalStrengthCdma = ((CellInfoCdma) cellInfo)
								.getCellSignalStrength();
						dbm = cellSignalStrengthCdma.getDbm();
					} else if (cellInfo instanceof CellInfoWcdma) {
						if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR2) {
							CellSignalStrengthWcdma cellSignalStrengthWcdma = ((CellInfoWcdma) cellInfo)
									.getCellSignalStrength();
							dbm = cellSignalStrengthWcdma.getDbm();
						}
					} else if (cellInfo instanceof CellInfoLte) {
						CellSignalStrengthLte cellSignalStrengthLte = ((CellInfoLte) cellInfo)
								.getCellSignalStrength();
						dbm = cellSignalStrengthLte.getDbm();
					}
				}
			}
		}
		return dbm;
	}
}
