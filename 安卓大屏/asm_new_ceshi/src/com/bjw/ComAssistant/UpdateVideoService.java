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
		Thread td3 = new Thread(downVideo, "downVideo");// 下载视频信息
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
	 * 下载视频 服务器端不存在的需要删除
	 * @author Administrator
	 *
	 */
	private class DownLoadVideo extends Thread {
		public void run() {
			try {
				// 判断本地是否有该视频 有的话就不需要下载 取出json串
				String videoInfo= Util.getSharePer(getApplicationContext(), "videoInfo");
				if(videoInfo==null&&"".equals(videoInfo))
				{
					return ;
				}
				JSONArray jsonArray = new JSONArray(videoInfo);
				for(int i=0;i<jsonArray.length();i++)
				{
					//判断横竖屏分开下载0 横屏 1竖屏
					JSONObject jsonObject = jsonArray.getJSONObject(i);
					String type=jsonObject.getString("type");
					String path=jsonObject.getString("path");
					String name=jsonObject.getString("name");
					HttpDownloader httpDownloader = new HttpDownloader();
					if("0".equals(type))
					{
						//横屏  // downloadResult 0下载成功1 文件已经存在-1 下载失败
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_HVIDEO_PATH,name);
					}else if("1".equals(type)) {
						//竖屏
						int downloadResult = httpDownloader.downloadFiles(path, Config.OPEN_VVIDEO_PATH,name);
					}
				}
				List<File> bdFileList = new ArrayList<File>();//存放本地视频列表
				// 循环文件夹下的所有视频如果本地有的视频videolist表内不存在 则把该视频删掉 横屏广告
				File f = new File(Util.getHVideoPath(getApplicationContext()));
				bdFileList = Util.getFile(f);
				for (int i = 0; i < bdFileList.size(); i++) {
					File file = bdFileList.get(i);
					//可以根据最新的json串是否包含本地存放的视频名称 如果不存在则删除
					if (!videoInfo.contains(file.getName())) {
						// 删掉本地的视频
						Util.deleteFile(Util.getHVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
				// 竖屏广告
				File vf = new File(Util.getVVideoPath(getApplicationContext()));
				List<File> vf_fileList = Util.getFile(vf);
				for (int i = 0; i < vf_fileList.size(); i++) {
					File file = vf_fileList.get(i);
					if (!videoInfo.contains(file.getName())) {
						// 删掉本地的视频
						Util.deleteFile(Util.getVVideoPath(getApplicationContext())
								+ file.getName());
					}
				}
			} catch (Exception e) {
				
			}
		}
	}
}
