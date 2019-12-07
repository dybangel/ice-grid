package com.sq.util;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

import android.util.Log;

public class FileUtils {
	private String SDCardRoot;

	public FileUtils(String path) {
		// �õ���ǰ�ⲿ�洢�豸��Ŀ¼
		// SDCardRoot= Environment.getExternalStorageDirectory()+File.separator;
		// File.separatorΪ�ļ��ָ�����/��,����֮����Ŀ¼�´����ļ�
		// SDCardRoot = context.getFilesDir().toString()+ File.separator;
		SDCardRoot = path;
	}

	// ��SD���ϴ����ļ�
	public File createFileInSDCard(String fileName, String dir)
			throws IOException {
		File file = new File(SDCardRoot + fileName);
		file.createNewFile();
		return file;
	}

	// ��SD���ϴ���Ŀ¼
	public File createSDDir(String dir) throws IOException {
		File dirFile = new File(SDCardRoot);
		dirFile.mkdir();// mkdir()ֻ�ܴ���һ���ļ�Ŀ¼��mkdirs()���Դ�������ļ�Ŀ¼
		return dirFile;
	}
/***
 * �ж��ļ��Ƿ����
 * @param strFile
 * @return
 */
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

	// �ж��ļ��Ƿ����
	public boolean isFileExist(String fileName, String dir) {
		File file = new File(SDCardRoot + fileName);
		return file.exists();
	}

	// ��һ��InoutStream���������д�뵽SD����
	public File write2SDFromInput(String fileName, String dir, InputStream input) {
		File file = null;
		OutputStream output = null;
		try {
			// ����Ŀ¼
			createSDDir(dir);
			// �����ļ�
			file = createFileInSDCard(fileName, dir);
			// д������
			output = new FileOutputStream(file);
			byte buffer[] = new byte[4 * 1024];// ÿ�δ�4K
			int temp;
			// д������
			while ((temp = input.read(buffer)) != -1) {
				output.write(buffer, 0, temp);
			}
			output.flush();
		} catch (Exception e) {
		} finally {
			try {
				output.close();
			} catch (Exception e2) {

			}
		}
		return file;
	}
}
