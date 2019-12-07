package com.sq.util;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;

import android.os.Environment;
import android.util.Log;

public class FileUtil {
	
	public static String readTxtFile()
	{
		try {  
		    File file = new File(Environment.getExternalStorageDirectory() + "/asm/","config.txt");  
		    BufferedReader br = new BufferedReader(new FileReader(file));  
		    String readline = "";  
		    StringBuffer sb = new StringBuffer();  
		    while ((readline = br.readLine()) != null) {  
		        sb.append(readline);  
		    }  
		    br.close();  
		    return sb.toString();
		} catch (Exception e) {  
		    e.printStackTrace();  
		}
		return null;
	}
	public static void writeTxtToFile(String strcontent) {
		FileOutputStream fos = null;
		// 生成文件夹之后，再生成文件，不然会出错
		String filePath=Environment.getExternalStorageDirectory() + "/asm/";
		String fileName="config.txt";
		makeFilePath(filePath, fileName);

		String strFilePath = filePath + fileName;
		// 每次写入时，都换行写
		try {
			File file = new File(strFilePath);
			if (!file.exists()) {
				file.getParentFile().mkdirs();
				file.createNewFile();
			}
			fos = new FileOutputStream(file);
			byte[] buffer = strcontent.getBytes();
			fos.write(buffer);
			fos.close();
		} catch (Exception e) {
			Log.e("TestFile", "Error on write File:" + e);
			try {
				if (fos != null) {
					fos.close();
				}
			} catch (Exception e1) {
				e1.printStackTrace();
			}
		}
	}

	/*
	 * 定义文件保存的方法，写入到文件中，所以是输出流
	 */
	public static void save(String fileName, String content) {
		FileOutputStream fos = null;
		try {
			/* 判断sd的外部设置状态是否可以读写 */
			if (Environment.getExternalStorageState().equals(
					Environment.MEDIA_MOUNTED)) {
				File file = new File(Environment.getExternalStorageDirectory(),
						fileName + ".txt");
				// 先清空内容再写入
				fos = new FileOutputStream(file);
				byte[] buffer = content.getBytes();
				fos.write(buffer);
				fos.close();
			}

		} catch (Exception ex) {

			ex.printStackTrace();

			try {
				if (fos != null) {
					fos.close();
				}
			} catch (Exception e) {
				e.printStackTrace();
			}
		}
	}

	// 生成文件夹
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
}
