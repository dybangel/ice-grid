package com.bjw.ComAssistant;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;

import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.HttpDownloader;
import com.sq.util.Util;

public class UpdateVideoService extends Service{

	@Override
	public IBinder onBind(Intent intent) {
		// TODO Auto-generated method stub
		return null;
	}
	@Override
	public void onCreate() {
		Thread td3 = new Thread(downVideo, "downVideo");// ������Ƶ��Ϣ
		td3.start();
		super.onCreate();
	}
	private Runnable downVideo = new Runnable() {
		@Override
		public void run() {
			while (true) {
				try {
					Dao dao = new Dao();
					dao.updateVideoList(Util.getSharePer(getApplicationContext(), "mechineID"),getApplicationContext());
					new DownLoadVideo().start();
					Thread.sleep(1000 * 60*5);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
		}
	};
	/**
	 * ������Ƶ �������˲����ڵ���Ҫɾ��
	 * @author Administrator
	 *
	 */
	private class DownLoadVideo extends Thread {
		public void run() {
			try {
				// �жϱ����Ƿ��и���Ƶ �еĻ��Ͳ���Ҫ���� ȡ��json��
				String videoInfo= Util.getSharePer(getApplicationContext(), "videoInfo");
				if(videoInfo==null&&"".equals(videoInfo))
				{
					return ;
				}
				JSONArray jsonArray = new JSONArray(videoInfo);
				for(int i=0;i<jsonArray.length();i++)
				{
					//�жϺ������ֿ�����0 ���� 1����
					JSONObject jsonObject = jsonArray.getJSONObject(i);
					String type=jsonObject.getString("type");
					String path=jsonObject.getString("path");
					String name=jsonObject.getString("name");
					HttpDownloader httpDownloader = new HttpDownloader();
					if("0".equals(type))
					{
						//����  // downloadResult 0���سɹ�1 �ļ��Ѿ�����-1 ����ʧ��
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_HVIDEO_PATH,name);
					}else if("1".equals(type)) {
						//����
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_VVIDEO_PATH,name);
					}
				}
				List<File> bdFileList = new ArrayList<File>();//��ű�����Ƶ�б�
				// ѭ���ļ����µ�������Ƶ��������е���Ƶvideolist���ڲ����� ��Ѹ���Ƶɾ�� �������
				File f = new File(Util.getHVideoPath(getApplicationContext()));
				bdFileList = Util.getFile(f);
				for (int i = 0; i < bdFileList.size(); i++) {
					File file = bdFileList.get(i);
					//���Ը������µ�json���Ƿ�������ش�ŵ���Ƶ���� �����������ɾ��
					if (!videoInfo.contains(file.getName())) {
						// ɾ�����ص���Ƶ
						Util.deleteFile(Util.getHVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
				// �������
				File vf = new File(Util.getVVideoPath(getApplicationContext()));
				List<File> vf_fileList = Util.getFile(vf);
				for (int i = 0; i < vf_fileList.size(); i++) {
					File file = vf_fileList.get(i);
					if (!videoInfo.contains(file.getName())) {
						// ɾ�����ص���Ƶ
						Util.deleteFile(Util.getVVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
			} catch (Exception e) {
				
			}
		}
	}
}
