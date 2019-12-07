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
		// �����ļ���֮���������ļ�����Ȼ�����
		String filePath=Environment.getExternalStorageDirectory() + "/asm/";
		String fileName="config.txt";
		makeFilePath(filePath, fileName);

		String strFilePath = filePath + fileName;
		// ÿ��д��ʱ��������д
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
	 * �����ļ�����ķ�����д�뵽�ļ��У������������
	 */
	public static void save(String fileName, String content) {
		FileOutputStream fos = null;
		try {
			/* �ж�sd���ⲿ����״̬�Ƿ���Զ�д */
			if (Environment.getExternalStorageState().equals(
					Environment.MEDIA_MOUNTED)) {
				File file = new File(Environment.getExternalStorageDirectory(),
						fileName + ".txt");
				// �����������д��
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

	// �����ļ���
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
