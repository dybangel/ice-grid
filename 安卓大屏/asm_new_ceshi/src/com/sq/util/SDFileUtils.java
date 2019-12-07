package com.sq.util;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;

import android.os.Environment;

public class SDFileUtils {
	private String SDPATH;
	private String fileName = "logFile.txt";
	private static SDFileUtils logFile = null;
	// SDCRAD�ļ����ʵĹ��캯��
	public SDFileUtils() {
		SDPATH = Environment.getExternalStorageDirectory() + "/";
	}

	public static SDFileUtils get() {
		if (logFile == null) {
			logFile = new SDFileUtils();
		}

		return logFile;
	}

	// ��SDCRAD�ϴ����ļ�
	private File createFile() throws IOException {
		File file = new File(SDPATH + fileName);
		file.createNewFile();
		return file;
	}

	// ���ļ���д������
	public void writeToSDFile(String msg) {
		File file = null;
		OutputStream outputStream = null;
		try {
			file = this.createFile();

			outputStream = new FileOutputStream(file, true);

			outputStream.write(msg.getBytes());

			outputStream.flush();

		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} finally {
			try {
				outputStream.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
}
