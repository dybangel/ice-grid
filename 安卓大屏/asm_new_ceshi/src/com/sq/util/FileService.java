package com.sq.util;

import java.io.ByteArrayOutputStream;
import java.io.FileInputStream;
import java.io.FileOutputStream;

import android.content.Context;
import android.util.Log;

public class FileService {
	public static final String TAG = "FileService";
	  private Context context;
	  //�õ�����������Ķ��������
	  public FileService(Context context) {
	   this.context = context;
	  }
	  /**
	   * �����ļ�
	   *
	   * @param fileName �ļ���
	   * @param content �ļ�����
	   * @throws Exception
	   */
	  public void save(String fileName, String content) throws Exception {
	   // ����ҳ������Ķ����ı���Ϣ�����Ե��ļ���������.txt��׺����βʱ���Զ�����.txt��׺
	   if (!fileName.endsWith(".txt")) {
	    fileName = fileName + ".txt";
	   }
	   byte[] buf = fileName.getBytes("iso8859-1");
	   Log.e(TAG, new String(buf,"utf-8"));
	   fileName = new String(buf,"utf-8");
	   Log.e(TAG, fileName);
	   // Context.MODE_PRIVATE��ΪĬ�ϲ���ģʽ��������ļ���˽�����ݣ�ֻ�ܱ�Ӧ�ñ�����ʣ��ڸ�ģʽ�£�д������ݻḲ��ԭ�ļ������ݣ���������д�������׷�ӵ�ԭ�ļ��С�����ʹ��Context.MODE_APPEND
	   // Context.MODE_APPEND��ģʽ�����ļ��Ƿ���ڣ����ھ����ļ�׷�����ݣ�����ʹ������ļ���
	   // Context.MODE_WORLD_READABLE��Context.MODE_WORLD_WRITEABLE������������Ӧ���Ƿ���Ȩ�޶�д���ļ���
	   // MODE_WORLD_READABLE����ʾ��ǰ�ļ����Ա�����Ӧ�ö�ȡ��MODE_WORLD_WRITEABLE����ʾ��ǰ�ļ����Ա�����Ӧ��д�롣
	   // ���ϣ���ļ�������Ӧ�ö���д�����Դ��룺
	   // openFileOutput("output.txt", Context.MODE_WORLD_READABLE + Context.MODE_WORLD_WRITEABLE);
	   FileOutputStream fos = context.openFileOutput(fileName, context.MODE_PRIVATE);
	   fos.write(content.getBytes());
	   fos.close();
	  }
	  /**
	   * ��ȡ�ļ�����
	   *
	   * @param fileName �ļ���
	   * @return �ļ�����
	   * @throws Exception
	   */
	  public String read(String fileName) throws Exception {
	   // ����ҳ������Ķ����ı���Ϣ�����Ե��ļ���������.txt��׺����βʱ���Զ�����.txt��׺
	   if (!fileName.endsWith(".txt")) {
	    fileName = fileName + ".txt";
	   }
	   FileInputStream fis = context.openFileInput(fileName);
	   ByteArrayOutputStream baos = new ByteArrayOutputStream();
	   byte[] buf = new byte[1024];
	   int len = 0;
	   //����ȡ������ݷ������ڴ���---ByteArrayOutputStream
	   while ((len = fis.read(buf)) != -1) {
	    baos.write(buf, 0, len);
	   }
	   fis.close();
	   baos.close();
	   //�����ڴ��д洢������
	   return baos.toString();
	  }
}
