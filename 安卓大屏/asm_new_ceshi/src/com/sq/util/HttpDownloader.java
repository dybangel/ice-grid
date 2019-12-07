package com.sq.util;

import java.io.BufferedReader;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import android.content.Context;
import android.util.Log;

public class HttpDownloader {
	String line = null;
	StringBuffer strBuffer = new StringBuffer();
	BufferedReader bufferReader = null;
	// ����С�͵��ĵ��ļ��������ĵ���String�ַ���
	public String downloadFiles(String urlStr) {
		try {
			InputStream inputStream = getInputStreamFromUrl(urlStr);
			bufferReader = new BufferedReader(new InputStreamReader(inputStream));
			while ((line = bufferReader.readLine()) != null) {
				strBuffer.append(line + '\n');
			}
		} catch (Exception e) {
			strBuffer.append("something is wrong!!");
		} finally {
			try {
				bufferReader.close();
			} catch (Exception e) {
				strBuffer.append("something is wrong!!");
				e.printStackTrace();
			}
		}
		return strBuffer.toString();
	}
	// �������������ļ�������MP3�������ļ��洢���ƶ�Ŀ¼��-1������ʧ�ܣ�0�����سɹ���1���ļ��Ѵ��ڣ�
	public int downloadFiles(String urlStr, String path, String fileName) {
		try {
			FileUtils fileUtils = new FileUtils(path);
			if (fileUtils.isFileExist(fileName, path)) {
				return 1;// �ж��ļ��Ƿ����
			} else {
				
				InputStream inputStream = getInputStreamFromUrl(urlStr);
				
				File resultFile = fileUtils.write2SDFromInput(fileName, path,inputStream);
				
				if (resultFile == null)
				{
					
					return -1;
				}
			}
		} catch (Exception e) {
			
			return -1;
		}
		return 0;
	}
	public int downloadFilesQR(String urlStr, String path, String fileName) {
		try {
			FileUtils fileUtils = new FileUtils(path);
			if (fileUtils.isFileExist(fileName, path))
			{
				return 1;// �ж��ļ��Ƿ����
			}
			else {
				InputStream inputStream = getInputStreamFromUrl(urlStr);
				File resultFile = fileUtils.write2SDFromInput(fileName, path,inputStream);
				if (resultFile == null)
				{
					return -1;
				}
			}
		} catch (Exception e) {
			return -1;
		}
		return 0;
	}
	public InputStream getInputStreamFromUrl(String urlStr) throws IOException {
		// ����һ��URL����
		URL url = new URL(urlStr);
		// ����һ��HTTP����
		HttpURLConnection urlConn = (HttpURLConnection) url.openConnection();
		// ʹ��IO����ȡ����
		InputStream inputStream = urlConn.getInputStream();
		return inputStream;
	}
}
