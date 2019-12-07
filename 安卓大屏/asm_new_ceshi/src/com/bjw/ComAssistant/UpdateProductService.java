package com.bjw.ComAssistant;

import java.io.File;
import java.util.ArrayList;
import java.util.List;

import org.json.JSONArray;
import org.json.JSONObject;

import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.HttpDownloader;
import com.sq.util.Util;

import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.util.Log;

public class UpdateProductService extends Service{

	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}
	@Override
	public void onCreate() {
		Thread td2 = new Thread(downProduct, "downProduct");// 下载商品信息
		//td2.start();
		Thread td3 = new Thread(downProductType, "downProductType");// 下载商品类别信息
		//td3.start();
		super.onCreate();
	}
	/***
	 * 更新产品
	 */
	private Runnable downProduct = new Runnable() {
		@Override
		public void run() {
			while (true) {
				try {
					Log.i("5555", "开始更新产品列表");
					Dao dao = new Dao();
					dao.updateProductList(Util.getSharePer(getApplicationContext(), "mechineBH"),getApplicationContext());
					down_productImg();
					final Intent intent = new Intent();
					intent.setAction(Config.ACTION_UPDATEUI_IMG);
					intent.putExtra("IMG_FLAG", true);
					sendBroadcast(intent);
					Log.i("5555", "更新完成发送广播");
					Thread.sleep(Config.update_produtct_time);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
		}
	};
	/***
	 * 更新产品类别
	 */
	private Runnable downProductType = new Runnable() {
		@Override
		public void run() {
			while (true) {
				try {
					
					Dao dao = new Dao();
					dao.updateProductType(getApplicationContext());
					final Intent intent = new Intent();
					intent.setAction(Config.ACTION_PRODUCTTYPE);
					intent.putExtra("TYPE_FLAG", true);
					sendBroadcast(intent);
					Thread.sleep(Config.update_productType_time);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
		}
	};
	/***
	 * 下载商品图片
	 */
	public void down_productImg() {
		try {
			HttpDownloader httpDownloader = new HttpDownloader();
			String productInfo= Util.getSharePer(getApplicationContext(), "productInfo");
			String companyID= Util.getSharePer(getApplicationContext(), "companyID");
			if(productInfo==null&&"".equals(productInfo))
			{
				return ;
			}
			List<File> bdFileList = new ArrayList<File>();
			File f = new File(Config.PRODUCT_IMAGE_SAVE_PATH);
			bdFileList = Util.getFile(f);
			for (int i = 0; i < bdFileList.size(); i++) {
				File file = bdFileList.get(i);
				if (!productInfo.contains(file.getName())) {
					// 删掉本地的视频
					Util.deleteFile(Config.PRODUCT_IMAGE_SAVE_PATH+ file.getName());
				}
			}
			JSONArray jsonArray = new JSONArray(productInfo);
			for(int i=0;i<jsonArray.length();i++)
			{
				JSONObject jsonObject = jsonArray.getJSONObject(i);
				String httpImageUrl=jsonObject.getString("httpImageUrl");
				String path=jsonObject.getString("path");
				int downloadResult = httpDownloader.downloadFiles(httpImageUrl, Config.PRODUCT_IMAGE_SAVE_PATH, path.replace("img/", ""));
				//下载二维码图片
				httpDownloader.downloadFiles("http://nq.bingoseller.com/qrcode/"+companyID+".png", Config.PRODUCT_IMAGE_SAVE_PATH_QR, companyID+".png");
			}
		} catch (Exception e) {
			
		}
	}
}
