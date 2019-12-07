package com.bjw.ComAssistant;

import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.nio.charset.Charset;

import org.json.JSONException;
import org.json.JSONObject;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.content.Intent;
import android.content.pm.PackageManager.NameNotFoundException;
import android.net.Uri;
import android.os.Build;
import android.os.Environment;
import android.os.Handler;
import android.os.Message;
import android.support.v4.content.FileProvider;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ProgressBar;

import com.sq.util.Config;
import com.sq.util.PackageUtils;
import com.sq.util.ShellUtils;
import com.sq.util.Util;

public class UpdateManager {
	private Context mContext;
	// 提示语
	private String updateMsg = "有最新的软件包哦，亲快下载吧~";
	// 返回的安装包url
	public String apkUrl = "";// 次数是服务器apk下载路径
	private Dialog noticeDialog;
	private Dialog downloadDialog;
	/* 下载包安装路径 */
	// private static final String savePath =
	// "/sdcard/updatedemo/";UpdateDemoRelease.apk
	private static final String savePath = Environment.getExternalStorageDirectory() + "/asm/";
	private static final String saveFileName = savePath + "UpdateDemoRelease.apk";
	/* 进度条与通知ui刷新的handler和msg常量 */
	private ProgressBar mProgress;
	private static final int DOWN_UPDATE = 1;
	private static final int DOWN_OVER = 2;
	private int progress;
	private Thread downLoadThread;
	private boolean interceptFlag = false;
	public String mechineID="";
	public static int getVerCode(Context context) {
		int verCode = -1;
		try {
			verCode = context.getPackageManager().getPackageInfo("com.bjw.ComAssistant", 0).versionCode;

		} catch (NameNotFoundException e) {

		}
		return verCode;
	}

	public static String getVerName(Context context) {
		String verName = "";
		try {
			verName = context.getPackageManager().getPackageInfo("com.bjw.ComAssistant", 0).versionName;
		} catch (NameNotFoundException e) {
		}
		return verName;
	}

	public int newVerCode;
	public String newVerName = "";
    
	boolean getServerVer(final Context context) {
		
	
		Log.i("3333", "newVerCode=" + newVerCode + ";newVerName=" + newVerName + "=="+ getVerCode(context)+";apkUrl="+apkUrl);
		if (newVerCode != getVerCode(context)) {
			// 更新
			// checkUpdateInfo();
			try {
				Util.debuglog("收到更新广播newvercode","PACKAGE_REPLACED");
				JSONObject jsonObject = new JSONObject();
				jsonObject.put("mechineID", mechineID);
				jsonObject.put("MsgId", "");
				jsonObject.put("cmd", "updateSoft");
				JSONObject samtypeJson= new JSONObject();
				samtypeJson.put(Config.VERSION, newVerName);
				samtypeJson.put(Config.VERCODE, newVerCode);
				samtypeJson.put("updateSoftStatus", 2);
				jsonObject.put("samtype", samtypeJson.toString());
				Util.sendMsg(jsonObject.toString());
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			downloadApk();
		} else {
		}
						
		return true;
	}
	

	private Handler mHandler = new Handler() {
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case DOWN_UPDATE:
				break;
			case DOWN_OVER:
//				boolean b = ShellUtils.checkRootPermission();
//				if (b) {
//					Log.i("3333", "开始安装1");
//					String apkPath = saveFileName;
//					int resultCode = PackageUtils.installSilent(mContext, apkPath);
//					if (resultCode != PackageUtils.INSTALL_SUCCEEDED) {
//						Log.i("3333", "开始安装="+resultCode);
//					}
//				}
				Util.debuglog("收到更新广播mhander","PACKAGE_REPLACED");
				try {
					JSONObject jsonObject = new JSONObject();
					jsonObject.put("mechineID", mechineID);
					jsonObject.put("MsgId", "");
					jsonObject.put("cmd", "updateSoft");
					JSONObject samtypeJson= new JSONObject();
					samtypeJson.put(Config.VERSION, newVerName);
					samtypeJson.put(Config.VERCODE, newVerCode);
					samtypeJson.put("updateSoftStatus", 3);
					jsonObject.put("samtype", samtypeJson.toString());
					Util.sendMsg(jsonObject.toString());
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				boolean b = clientInstall();
				Log.i("1010", "b=" + b);
				if (b) {
					
					
					Log.i("1010", "startApp");
					startApp("data/data/com.bjw.ComAssistant/", "IndexActivity");
				}
				break;
			default:
				break;
			}
		};
	};

	public UpdateManager(Context context) {
		this.mContext = context;

	}

	// 外部接口让主Activity调用
	public void checkUpdateInfo() {
		showNoticeDialog();
	}

	private void showNoticeDialog() {
		AlertDialog.Builder builder = new Builder(mContext);
		builder.setTitle("软件版本更新");
		builder.setMessage(updateMsg);
		builder.setPositiveButton("下载", new OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.dismiss();
				showDownloadDialog();
			}
		});
		builder.setNegativeButton("以后再说", new OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.dismiss();
			}
		});
		noticeDialog = builder.create();
		noticeDialog.show();
	}

	private void showDownloadDialog() {
		AlertDialog.Builder builder = new Builder(mContext);
		builder.setTitle("软件版本更新");
		final LayoutInflater inflater = LayoutInflater.from(mContext);
		View v = inflater.inflate(R.layout.progress, null);
		mProgress = (ProgressBar) v.findViewById(R.id.progress);
		builder.setView(v);
		builder.setNegativeButton("取消", new OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int which) {
				dialog.dismiss();
				interceptFlag = true;
			}
		});
		downloadDialog = builder.create();
		downloadDialog.show();
		downloadApk();
	}

	private Runnable mdownApkRunnable = new Runnable() {
		@Override
		public void run() {

			try {
				Util.debuglog("收到更新广播runnable","PACKAGE_REPLACED");
				URL url = new URL(apkUrl);
				HttpURLConnection conn = (HttpURLConnection) url.openConnection();
				conn.connect();
				int length = conn.getContentLength();
				InputStream is = conn.getInputStream();
				File file = new File(savePath);
				if (!file.exists()) {
					file.mkdir();
				}
				String apkFile = saveFileName;
				File ApkFile = new File(apkFile);
				FileOutputStream fos = new FileOutputStream(ApkFile);
				int count = 0;
				byte buf[] = new byte[1024];
				do {
					int numread = is.read(buf);
					count += numread;
					progress = (int) (((float) count / length) * 100);
					// 更新进度
					mHandler.sendEmptyMessage(DOWN_UPDATE);
					if (numread <= 0) {
						// 下载完成通知安装
						mHandler.sendEmptyMessage(DOWN_OVER);
						break;
					}
					fos.write(buf, 0, numread);
				} while (!interceptFlag);// 点击取消就停止下载.
				fos.close();
				is.close();
			} catch (MalformedURLException e) {
				e.printStackTrace();
			} catch (IOException e) {
				e.printStackTrace();
			}
		}
	};

	/**
	 * 下载apk
	 * 
	 * @param url
	 */
	private void downloadApk() {
		downLoadThread = new Thread(mdownApkRunnable);
		downLoadThread.start();
	}
//	public  boolean clientInstall() {
//		String apkPath = saveFileName;
//        boolean result = false;
//        DataOutputStream dataOutputStream = null;
//        BufferedReader errorStream = null;
//        BufferedReader successStream = null;
//        Process process = null;
//        Log.i("7777","1111");
//        try {
//            // 申请 su 权限
//            process = Runtime.getRuntime().exec("su");
//            Log.i("7777","2222");
//            dataOutputStream = new DataOutputStream(process.getOutputStream());
//            Log.i("7777","33333");
//            // 执行 pm install 命令
//            String command = "pm install -r " + apkPath + "\n";
//            dataOutputStream.write(command.getBytes(Charset.forName("UTF-8")));
//            dataOutputStream.writeBytes("exit\n");
//            dataOutputStream.flush();
//            Log.i("7777","44444");
//            process.waitFor();
//            Log.i("7777","55555");
//            errorStream = new BufferedReader(new InputStreamReader(process.getErrorStream()));
//            StringBuilder errorMsg = new StringBuilder();
//            String line;
//            while ((line = errorStream.readLine()) != null) {
//                errorMsg.append(line);
//            }
//            Log.i("7777","silent install error message:{"+errorMsg+"}");
//            StringBuilder successMsg = new StringBuilder();
//            successStream = new BufferedReader(new InputStreamReader(process.getInputStream()));
//            // 读取命令执行结果
//            while ((line = successStream.readLine()) != null) {
//                successMsg.append(line);
//            }
//            Log.i("7777","silent install success message:{"+successMsg+"}");
//            // 如果执行结果中包含 Failure 字样就认为是操作失败，否则就认为安装成功
//            if (!(errorMsg.toString().contains("Failure") || successMsg.toString().contains("Failure"))) {
//                result = true;
//            }
//        } catch (Exception e) {
//           Log.i("7777", "e="+e.getMessage());
//        } finally {
//            try {
//                if (process != null) {
//                    process.destroy();
//                }
//                if (dataOutputStream != null) {
//                    dataOutputStream.close();
//                }
//                if (errorStream != null) {
//                    errorStream.close();
//                }
//                if (successStream != null) {
//                    successStream.close();
//                }
//            } catch (Exception e) {
//                 
//            }
//        }
//        return result;
//    }
	public boolean clientInstall() {
		Util.debuglog("收到更新广播install","PACKAGE_REPLACED");
		String apkPath = saveFileName;
		PrintWriter PrintWriter = null;
		Process process = null;
		Log.i("3333", "安装1");
		try {
			process = Runtime.getRuntime().exec("su");
			Log.i("3333", "安装2");
			PrintWriter = new PrintWriter(process.getOutputStream());
			Log.i("3333", "安装3");
			PrintWriter.println("chmod 777 " + apkPath);
			Log.i("3333", "安装4");
			PrintWriter.println("export LD_LIBRARY_PATH=/vendor/lib:/system/lib");
			PrintWriter.println("pm install -r -d " + apkPath);
		    PrintWriter.println("exit");
			PrintWriter.flush();
			PrintWriter.close();
			Log.i("3333", "安装5");
			int value = process.waitFor();
			Log.i("3333", "安装6=" + value);
			return returnResult(value);
		} catch (Exception e) {
			Log.i("3333", "安装7=" + e.getMessage());
			e.printStackTrace();
		} finally {
			if (process != null) {
				process.destroy();
			}
		}
		return false;
	}
//	public boolean clientInstall() {
//		String apkPath = saveFileName;
//		Log.i("3333", "安装1");
//		PrintWriter PrintWriter = null;
//		Process process = null;
//		try {
//			process = Runtime.getRuntime().exec("su");
//			Log.i("3333", "安装2");
//			PrintWriter = new PrintWriter(process.getOutputStream());
//			Log.i("3333", "安装3");
//			PrintWriter.println("chmod 777 " + apkPath);
//			PrintWriter.println("export LD_LIBRARY_PATH=/vendor/lib:/system/lib");
//			PrintWriter.println("pm install -r " +apkPath);
//			PrintWriter.flush();
//			PrintWriter.close();
//			Log.i("3333", "安装4");
//			int value = process.waitFor();
//			Log.i("3333", "安装5=" + value);
//			return returnResult(value);
//		} catch (Exception e) {
//			Log.i("3333", "安装6=" + e.getMessage());
//			e.printStackTrace();
//		} finally {
//			if (process != null) {
//				process.destroy();
//			}
//		}
//		return false;
//	}

//	public boolean clientInstall() {
//		String command="pm install -r  "+saveFileName;
//		Process process = null;
//        BufferedReader bufferedReader = null;
//        StringBuilder mShellCommandSB =new StringBuilder();
//        Log.d("wenfeng", "runShellCommand :" + command);
//        mShellCommandSB.delete(0, mShellCommandSB.length());
//        String[] cmd = new String[] { "/system/bin/sh", "-c", command }; //调用bin文件
//        try {
//            byte b[] = new byte[1024];
//            process = Runtime.getRuntime().exec(cmd);
//            bufferedReader = new BufferedReader(new InputStreamReader(
//                    process.getInputStream()));
//            String line;
//            while ((line = bufferedReader.readLine()) != null) {
//                mShellCommandSB.append(line);
//            }
//            Log.d("wenfeng", "runShellCommand result : " + mShellCommandSB.toString());
//            process.waitFor();
//            return true;
//        } catch (IOException e) {
//            e.printStackTrace();
//        } catch (InterruptedException e) {
//            e.printStackTrace();
//        } finally {
//            if (bufferedReader != null) {
//                try {
//                    bufferedReader.close();
//                } catch (IOException e) {
//                    // TODO: handle exception
//                }
//            }
//
//            if (process != null) {
//                process.destroy();
//            }
//        }
//        return false;
//	}
	
	private static boolean returnResult(int value) {
		Log.i("3333", "returnResult");
		// 代表成功
		if (value == 0) {
			return true;
		} else if (value == 1) { // 失败
			return false;
		} else { // 未知情况
			return false;
		}
	}

	/**
	 * 启动app com.exmaple.client/.MainActivity
	 * com.exmaple.client/com.exmaple.client.MainActivity
	 */
	public static boolean startApp(String packageName, String activityName) {
		boolean isSuccess = false;
		String cmd = "am start -n " + packageName + "/" + activityName + " \n";
		Log.i("1010", "启动报错=" + cmd);
		Process process = null;
		try {
			process = Runtime.getRuntime().exec(cmd);
			Log.i("1010", "启动报错=1");
			int value = process.waitFor();
			Log.i("1010", "启动报错=2=" + value);
			return returnResult(value);
		} catch (Exception e) {
			Log.i("1010", "启动报错=" + e.getMessage());
			e.printStackTrace();
		} finally {
			if (process != null) {
				process.destroy();
			}
		}
		return isSuccess;
	}
}
