package com.sq.util;

import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.RandomAccessFile;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.Random;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

import net.sf.json.util.NewBeanInstanceStrategy;

import org.json.JSONException;
import org.json.JSONObject;

import com.bjw.ComAssistant.PortService;

import android.content.Context;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.AsyncTask;
import android.os.Environment;
import android.test.PerformanceTestCase;
import android.text.TextUtils;
import android.util.Base64;
import android.util.Log;

public class Util {
	private static final String TAG = "upload";
	public static String filePath = Config.LOG_URL;
	public static String billPath = Config.BILL_URL;
	
	public static String fileName = "log.txt";
	public static String appID = "9";
	public static String key = "a33f3d7c6b0ebea2ebd96c78a337c426bddbca14";
	public static String payurl = "https://pay.vyicoo.com/v3";

	public static String getTemper(String tem) {
		String tenjinzhi = Integer.valueOf(tem, 16).toString();
		String two = Integer.toBinaryString(Integer.parseInt(tenjinzhi));
		String result = String.format("%08d", Integer.parseInt(two));
		if (result.substring(0, 1).equals("1")) {
			String str = result.substring(1);
			str = str.replace("1", "2");
			str = str.replace("0", "1");
			str = str.replace("2", "0");
			str = "0" + str;
			return "-" + (Integer.valueOf(str, 2) + 1);
		} else {
			return tenjinzhi;
		}
	}
	public static void writeBillTxtToFile(String strcontent) {
		//打印到前台页面上用的
		//PortService.sendchlc(strcontent);
		String billno=Config.billno;
		SimpleDateFormat sfDateFormat=new SimpleDateFormat("yyyyMMdd");
		SimpleDateFormat sfDateFormat1=new SimpleDateFormat("yyyyMMdd hh:mm:ss SSS");
	
		makeFilePath(billPath+sfDateFormat.format(new Date())+"/", billno+fileName);
		String strFilePath = billPath+sfDateFormat.format(new Date())+"/" + billno+fileName;
		String strContent = sfDateFormat1.format(new Date())+":"+strcontent + "\r\n";
		try {
			File file = new File(strFilePath);
			if (!file.exists()) {
				file.getParentFile().mkdirs();
				file.createNewFile();
			}
			RandomAccessFile raf = new RandomAccessFile(file, "rwd");
			raf.seek(file.length());
			raf.write(strContent.getBytes());
			raf.close();
		} catch (Exception e) {

		}
	}
	
	public static void debuglog(String strcontent,String filename) {	
		SimpleDateFormat sfDateFormat=new SimpleDateFormat("yyyyMMdd");
		SimpleDateFormat sfDateFormat1=new SimpleDateFormat("yyyyMMdd hh:mm:ss SSS");	
		makeFilePath(billPath+sfDateFormat.format(new Date())+"/", filename+fileName);
		String strFilePath = billPath+sfDateFormat.format(new Date())+"/" + filename+fileName;
		String strContent = sfDateFormat1.format(new Date())+":"+strcontent + "\r\n";
		try {
			File file = new File(strFilePath);
			if (!file.exists()) {
				file.getParentFile().mkdirs();
				file.createNewFile();
			}
			RandomAccessFile raf = new RandomAccessFile(file, "rwd");
			raf.seek(file.length());
			raf.write(strContent.getBytes());
			raf.close();
		} catch (Exception e) {

		}
	}
	public static void writeBillErrTxtToFile(String strcontent) {
		String billno=Config.BILL_NO;
		SimpleDateFormat sfDateFormat=new SimpleDateFormat("yyyyMMdd");
		SimpleDateFormat sfDateFormat1=new SimpleDateFormat("yyyyMMdd hh:mm:ss SSS");
	
		makeFilePath(billPath+sfDateFormat.format(new Date())+"/", "err"+fileName);
		String strFilePath = billPath+sfDateFormat.format(new Date())+"/" + "err"+fileName;
		String strContent = sfDateFormat1.format(new Date())+":"+strcontent + "\r\n";
		try {
			File file = new File(strFilePath);
			if (!file.exists()) {
				file.getParentFile().mkdirs();
				file.createNewFile();
			}
			RandomAccessFile raf = new RandomAccessFile(file, "rwd");
			raf.seek(file.length());
			raf.write(strContent.getBytes());
			raf.close();
		} catch (Exception e) {

		}
	}
	public static void writeTxtToFile(String strcontent) {
		
		SimpleDateFormat sfDateFormat=new SimpleDateFormat("yyyyMMdd");
		makeFilePath(filePath, sfDateFormat.format(new Date())+fileName);
		String strFilePath = filePath + sfDateFormat.format(new Date())+""+fileName;
		String strContent = strcontent + "\r\n";
		try {
			File file = new File(strFilePath);
			if (!file.exists()) {
				file.getParentFile().mkdirs();
				file.createNewFile();
			}
			RandomAccessFile raf = new RandomAccessFile(file, "rwd");
			raf.seek(file.length());
			raf.write(strContent.getBytes());
			raf.close();
		} catch (Exception e) {

		}
	}

	public static File makeFilePath(String filePath, String fileName) {
		File file = null;
		makeRootDirectory(filePath);
		try {
			file = new File(filePath + fileName);
			if (!file.exists()) {
				file.createNewFile();
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return file;
	}

	public static void makeRootDirectory(String filePath) {
		File file = null;
		try {
			file = new File(filePath);
			if (!file.exists()) {
				file.mkdir();
			}
		} catch (Exception e) {
			Log.i("error:", e + "");
		}
	}

	public static String getDate2() {
		return new SimpleDateFormat("yyyy-MM-dd HH:mm:ss SSS")
				.format(new Date());
	}
	 
	/**
	 * ����ֻ�ֱ��ʴ�DPת��PX
	 * 
	 * @param context
	 * @param dpValue
	 * @return
	 */
	public static int dip2px(Context context, float dpValue) {
		float scale = context.getResources().getDisplayMetrics().density;
		return (int) (dpValue * scale + 0.5f);
	}

	/**
	 * ��spֵת��Ϊpxֵ����֤���ִ�С����
	 * 
	 * @param spValue
	 * @return
	 */
	public static int sp2px(Context context, float spValue) {
		final float fontScale = context.getResources().getDisplayMetrics().scaledDensity;
		return (int) (spValue * fontScale + 0.5f);
	}

	/**
	 * ����ֻ�ķֱ���PX(����)ת��DP
	 * 
	 * @param context
	 * @param pxValue
	 * @return
	 */
	public static int px2dip(Context context, float pxValue) {
		float scale = context.getResources().getDisplayMetrics().density;
		return (int) (pxValue / scale + 0.5f);
	}

	/**
	 * ��pxֵת��Ϊspֵ����֤���ִ�С����
	 * 
	 * @param pxValue
	 * @return
	 */
	public static int px2sp(Context context, float pxValue) {
		final float fontScale = context.getResources().getDisplayMetrics().scaledDensity;
		return (int) (pxValue / fontScale + 0.5f);
	}

	// �ж��ļ����Ƿ���� �������򴴽�
	public static boolean isFolderExists(String strFolder) {
		File file = new File(strFolder);

		if (!file.exists()) {
			if (file.mkdir()) {
				return true;
			} else
				return false;
		}
		return true;
	}

	// �ж��ļ��Ƿ����
	public static boolean fileIsExists(String strFile) {
		try {
			File f = new File(strFile);
			if (!f.exists()) {
				return false;
			}
		} catch (Exception e) {
			return false;
		}
		return true;
	}

	// ��ȡĳ���ļ��������е��ļ�
	public static List<File> getFile(File file) {
		List<File> mFileList = new ArrayList<File>();
		File[] fileArray = file.listFiles();
		for (File f : fileArray) {
			if (f.isFile()) {
				mFileList.add(f);
			} else {
				getFile(f);
			}
		}
		return mFileList;
	}

	/**
	 * ɾ����ļ�
	 * 
	 * @param fileNameҪɾ
	 *            ����ļ����ļ���
	 * @return �����ļ�ɾ��ɹ�����true�����򷵻�false
	 */
	public static boolean deleteFile(String fileName) {
		File file = new File(fileName);
		// ����ļ�·�����Ӧ���ļ����ڣ�������һ���ļ�����ֱ��ɾ��
		if (file.exists() && file.isFile()) {
			if (file.delete()) {
				return true;
			} else {
				return false;
			}
		} else {
			return false;
		}
	}
	/**
	   * 压缩文件和文件夹
	   *
	   * @param srcFileString 要压缩的文件或文件夹
	   * @param zipFileString 压缩完成的Zip路径
	   * @throws Exception
	   */
	  public static void ZipFolder(String srcFileString, String zipFileString) throws Exception {
	    //创建ZIP
	    ZipOutputStream outZip = new ZipOutputStream(new FileOutputStream(zipFileString));
	    //创建文件
	    File file = new File(srcFileString);
	    //压缩
	   
	    ZipFiles(file.getParent()+ File.separator, file.getName(), outZip);
	    //完成和关闭
	    outZip.finish();
	    outZip.close();
	  }
	  
	  /**
	   * 压缩文件
	   *
	   * @param folderString
	   * @param fileString
	   * @param zipOutputSteam
	   * @throws Exception
	   */
	  private static void ZipFiles(String folderString, String fileString, ZipOutputStream zipOutputSteam) throws Exception {
	    
	    if (zipOutputSteam == null)
	      return;
	    File file = new File(folderString + fileString);
	    if (file.isFile()) {
	      ZipEntry zipEntry = new ZipEntry(fileString);
	      FileInputStream inputStream = new FileInputStream(file);
	      zipOutputSteam.putNextEntry(zipEntry);
	      int len;
	      byte[] buffer = new byte[4096];
	      while ((len = inputStream.read(buffer)) != -1) {
	        zipOutputSteam.write(buffer, 0, len);
	      }
	      zipOutputSteam.closeEntry();
	    } else {
	      //文件夹
	      String fileList[] = file.list();
	      //没有子文件和压缩
	      if (fileList.length <= 0) {
	        ZipEntry zipEntry = new ZipEntry(fileString + File.separator);
	        zipOutputSteam.putNextEntry(zipEntry);
	        zipOutputSteam.closeEntry();
	      }
	      //子文件和递归
	      for (int i = 0; i < fileList.length; i++) {
	        ZipFiles(folderString+fileString+"/", fileList[i], zipOutputSteam);
	      }
	    }
	}
  /** 删除文件，可以是文件或文件夹
     * @param delFile 要删除的文件夹或文件名
     * @return 删除成功返回true，否则返回false
     */
	  public static boolean delete(String delFile) {
        File file = new File(delFile);
        if (!file.exists()) {
           
            return false;
        } else {
            if (file.isFile())
                return deleteSingleFile(delFile);
            else
                return deleteDirectory(delFile);
        }
    }
 
    /** 删除单个文件
     * @param filePath$Name 要删除的文件的文件名
     * @return 单个文件删除成功返回true，否则返回false
     */
    private static boolean deleteSingleFile(String filePath$Name) {
        File file = new File(filePath$Name);
        // 如果文件路径所对应的文件存在，并且是一个文件，则直接删除
        if (file.exists() && file.isFile()) {
            if (file.delete()) {
               
                return true;
            } else {
              
                return false;
            }
        } else {
           
            return false;
        }
    }
 
    /** 删除目录及目录下的文件
     * @param filePath 要删除的目录的文件路径
     * @return 目录删除成功返回true，否则返回false
     */
    private static boolean deleteDirectory(String filePath) {
        // 如果dir不以文件分隔符结尾，自动添加文件分隔符
        if (!filePath.endsWith(File.separator))
            filePath = filePath + File.separator;
        File dirFile = new File(filePath);
        // 如果dir对应的文件不存在，或者不是一个目录，则退出
        if ((!dirFile.exists()) || (!dirFile.isDirectory())) {
           
            return false;
        }
        boolean flag = true;
        // 删除文件夹中的所有文件包括子目录
        File[] files = dirFile.listFiles();
        for (File file : files) {
            // 删除子文件
            if (file.isFile()) {
                flag = deleteSingleFile(file.getAbsolutePath());
                if (!flag)
                    break;
            }
            // 删除子目录
            else if (file.isDirectory()) {
                flag = deleteDirectory(file
                        .getAbsolutePath());
                if (!flag)
                    break;
            }
        }
        if (!flag) {
           
            return false;
        }
        // 删除当前目录
        if (dirFile.delete()) {
         
            return true;
        } else {
         
            return false;
        }
    }
	public static String getImagePath(Context context) {
		String filesDir = context.getFilesDir().toString() + "/image/";
		Util.isFolderExists(filesDir);
		return filesDir;
	}

	public static String getHVideoPath(Context context) {
		String filesDir = Config.OPEN_HVIDEO_PATH;
		Util.isFolderExists(filesDir);
		return filesDir;
	}

	public static String getVVideoPath(Context context) {
		String filesDir = Config.OPEN_VVIDEO_PATH;
		Util.isFolderExists(filesDir);
		return filesDir;
	}

	public static String getDate() {
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
				"yyyy-MM-dd HH:mm:ss");// HH:mm:ss
		Date date = new Date(System.currentTimeMillis());
		return simpleDateFormat.format(date);
	}
	public static String getDate3() {
		SimpleDateFormat simpleDateFormat = new SimpleDateFormat(
				"yyyy-MM-dd HH:mm:ss:SSS");// HH:mm:ss
		Date date = new Date(System.currentTimeMillis());
		return simpleDateFormat.format(date);
	}
	public static void setProperties(Context context, String keyName,
			String keyValue) {
		Properties properties = new Properties();
		try {
			String pathString=Environment.getExternalStorageDirectory()+"/asm/";
			FileOutputStream fos = new FileOutputStream(new File(
					pathString, "config"), true);
			properties.setProperty(keyName, keyValue);
			properties.store(fos, null);
			fos.flush();
			fos.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public static String getProperties(Context context, String keyName) {
		Properties properties = new Properties();
		try {
			String pathString=Environment.getExternalStorageDirectory()+"/asm/";
			FileInputStream fis = new FileInputStream(new File(
					pathString, "config"));
			properties.load(fis);
			fis.close();
		} catch (Exception e) {
			e.printStackTrace();
		}

		return properties.getProperty(keyName);
	}
	public static void setSharePer(Context context,String key,String value){
		 SharedPreferences preference=context.getSharedPreferences("config", Context.MODE_PRIVATE);
		 Editor editor= preference.edit();
		 editor.putString(key, value);
		 editor.commit();
	}
	public static String getSharePer(Context context, String key) {
		 SharedPreferences preference=context.getSharedPreferences("config", Context.MODE_PRIVATE);
		 String str= preference.getString(key, null);
		 return str;
	}
 
	public static Date getNextDay(Date date) {
		Calendar calendar = Calendar.getInstance();
		calendar.setTime(date);
		calendar.add(Calendar.DAY_OF_MONTH, 0);
		date = calendar.getTime();
		return date;
	}

	public static String getDateTime() {
		SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd");
		Date curDate = new Date(System.currentTimeMillis());// ��ȡ��ǰʱ��
		String str = formatter.format(curDate);
		return str;
	}

	public static void uploadFile(final File file, final String fName,
			final String urlStr) {
		new AsyncTask<String, Void, String>() {
			@Override
			protected String doInBackground(String... arg0) {
				// �첽���أ�ǧ���ܰ������������UI���߳��У���Ȼ�ᷢ���쳣android.os.NetworkOnMainThreadException
				String end = "\r\n";// �س�����
				String twoHyphens = "--";// ����ָ��ps:��boundary�ָ��Ĳ���
				String boundary = "***********";// �ֽ��߿����������
				try {
					URL url = new URL(urlStr);
					HttpURLConnection httpURLConnection = (HttpURLConnection) url
							.openConnection();
					httpURLConnection.setDoInput(true);
					httpURLConnection.setDoOutput(true);
					httpURLConnection.setUseCaches(false);// �����û���
					httpURLConnection.setRequestMethod("POST");// post����
					httpURLConnection.setRequestProperty("Connection",
							"keep-Alive");// һֱ����t��״̬
					httpURLConnection.setRequestProperty("Charset", "utf-8");// �ַ����Ϊutf-8
					httpURLConnection.setRequestProperty("Content-Type",
							"multipart/form-data;boundary=" + boundary);
					// �������Ϊ��Ԫ����ݣ�����ֻ�÷ֽ��߲��÷ָ���ʾ�������ϸ�������д����Ȼ�������޷�ʶ��
					DataOutputStream dataOutputStream = new DataOutputStream(
							httpURLConnection.getOutputStream());
					// ��ȡ�����
					String newName = fName;// ��ʱ�ļ����֡��������.�ҵķ�����˴洢�Ĳ�������Ϊ������ȫ��Ψһ��ʶ��Guid��4�����
					dataOutputStream.writeBytes(twoHyphens + boundary + end);
					dataOutputStream
							.writeBytes("Content-Disposition: form-data; "
									+ "name=\"MyHeadPicture\";filename=\""
									+ newName + "\"" + end);
					/*
					 * ע�⣬ǧ��ע�����MyHeadPicture�������˵�<input type="file"
					 * name="MyHeadPicture"/>name��Ӧ������һ��
					 * ����ס�������˻س����н����־
					 */
					dataOutputStream.writeBytes(end);
					FileInputStream fStream = new FileInputStream(file);// ��ȡ�����ļ�������
					/* ����ÿ��д��1024bytes */
					int bufferSize = 1024;
					byte[] buffer = new byte[bufferSize];
					int length = -1;
					// StringBuffer sb = new StringBuffer();
					/* ���ļ���ȡ����{����� */
					while ((length = fStream.read(buffer)) != -1) {
						// sb.append(length);
						/* ������д��DataOutputStream�� */
						dataOutputStream.write(buffer, 0, length);// ���ļ�һ�ֽ�����ʽ���뵽�������
					}
					dataOutputStream.writeBytes(end);
					dataOutputStream.writeBytes(twoHyphens + boundary
							+ twoHyphens + end);
					/* close streams */
					fStream.close();
					dataOutputStream.flush();
					/* ȡ��Response���� */
					InputStream is = httpURLConnection.getInputStream();// ���������Ӧ
					int ch;
					StringBuffer b = new StringBuffer();
					while ((ch = is.read()) != -1) {
						b.append((char) ch);
					}
					dataOutputStream.close();
					return b.toString();// ������Ӧ����
				} catch (IOException e) {
					e.printStackTrace();
				}
				return null;
			}

			@Override
			protected void onPostExecute(String result) {
				super.onPostExecute(result);
			}
		}.execute("");
	}

	/* �������ⰴ�� */
	public boolean hideNavigation() {
		boolean ishide;
		try {
			String command;
			command = "LD_LIBRARY_PATH=/vendor/lib:/system/lib service call activity 42 s16 com.android.systemui";
			Process proc = Runtime.getRuntime().exec(
					new String[] { "su", "-c", command });
			proc.waitFor();
			ishide = true;
		} catch (Exception ex) {
			// Toast.makeText(getApplicationContext(), ex.getMessage(),
			// Toast.LENGTH_LONG).show();
			ishide = false;
		}
		return ishide;
	}

	public static String getXLH() {
		boolean b = true;
		while (b) {
			Random random = new Random();
			String a = (random.nextInt(89) + 10) + "";
			String xlh = FileUtil.readTxtFile();
			if (xlh != null && xlh.length() != 0) {
				if (a.equals(xlh)) {
					b = true;
				} else {
					b = false;
					FileUtil.writeTxtToFile(a);
				}
			} else {
				b = false;
				FileUtil.writeTxtToFile(a);
			}
		}
		return FileUtil.readTxtFile();
	}

	/* ��ʾ���ⰴ�� */
	public boolean showNavigation() {
		boolean isshow;
		try {
			String command;
			command = "LD_LIBRARY_PATH=/vendor/lib:/system/lib am startservice -n com.android.systemui/.SystemUIService";
			Process proc = Runtime.getRuntime().exec(
					new String[] { "su", "-c", command });
			proc.waitFor();
			isshow = true;
		} catch (Exception e) {
			isshow = false;
			e.printStackTrace();
		}
		return isshow;
	}

	public static int compare_date(String DATE1, String DATE2) {
		SimpleDateFormat df = new SimpleDateFormat("HH:mm");
		try {
			Date dt1 = df.parse(DATE1);
			Date dt2 = df.parse(DATE2);
			if (dt1.getTime() > dt2.getTime()) {
				return 1;
			} else if (dt1.getTime() < dt2.getTime()) {
				return -1;
			} else {
				return 0;
			}
		} catch (Exception exception) {
			exception.printStackTrace();
		}
		return 0;
	}

	public static Boolean sendMsg(String s) {
		if (null != Config.webSocketConnection
				&& Config.webSocketConnection.isConnected()) {
			try {
				Config.webSocketConnection.sendTextMessage(s);
			} catch (Exception e) {
			}
			return true;
		}else{
			return false;
		}
	}

	public static String getVerName(Context context) {
		String verName = "";
		try {
			verName = context.getPackageManager().getPackageInfo(
					"com.bjw.ComAssistant", 0).versionName;
		} catch (NameNotFoundException e) {
		}
		return verName;
	}

	public static String imageToBase64(String path) {
		if (TextUtils.isEmpty(path)) {
			return null;
		}
		InputStream is = null;
		byte[] data = null;
		String result = null;
		try {
			is = new FileInputStream(path);
			// 创建一个字符流大小的数组。
			data = new byte[is.available()];
			// 写入数组
			is.read(data);
			// 用默认的编码格式进行编码
			result = Base64.encodeToString(data, Base64.DEFAULT);
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			if (null != is) {
				try {
					is.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}

		}
		return result;
	}
	//判断文件夹是否存在
	public static void isExist(String path) {
		File file = new File(path);
		// 判断文件夹是否存在,如果不存在则创建文件夹
		if (!file.exists()) {
			file.mkdir();
		}
	}
	public static void sendLog(Context context,String msg)
	{
		JSONObject reqJson = new JSONObject();
		try {
			reqJson.put(Config.CMD, Config.LOG);
			reqJson.put(Config.MECHINE_ID, getSharePer(context, "mechineID"));
			reqJson.put("msg", msg);
			reqJson.put("MsgId", "");
			Util.sendMsg(reqJson.toString());
		} catch (JSONException e) {
			e.printStackTrace();
		}
	}
	
}
