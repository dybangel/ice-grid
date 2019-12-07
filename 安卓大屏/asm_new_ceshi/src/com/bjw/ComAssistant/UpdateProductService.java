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
		Thread td2 = new Thread(downProduct, "downProduct");// ������Ʒ��Ϣ
		//td2.start();
		Thread td3 = new Thread(downProductType, "downProductType");// ������Ʒ�����Ϣ
		//td3.start();
		super.onCreate();
	}
	/***
	 * ���²�Ʒ
	 */
	private Runnable downProduct = new Runnable() {
		@Override
		public void run() {
			while (true) {
				try {
					Log.i("5555", "��ʼ���²�Ʒ�б�");
					Dao dao = new Dao();
					dao.updateProductList(Util.getSharePer(getApplicationContext(), "mechineBH"),getApplicationContext());
					down_productImg();
					final Intent intent = new Intent();
					intent.setAction(Config.ACTION_UPDATEUI_IMG);
					intent.putExtra("IMG_FLAG", true);
					sendBroadcast(intent);
					Log.i("5555", "������ɷ��͹㲥");
					Thread.sleep(Config.update_produtct_time);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
		}
	};
	/***
	 * ���²�Ʒ���
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
	 * ������ƷͼƬ
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
					// ɾ�����ص���Ƶ
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
				//���ض�ά��ͼƬ
				httpDownloader.downloadFiles("http://nq.bingoseller.com/qrcode/"+companyID+".png", Config.PRODUCT_IMAGE_SAVE_PATH_QR, companyID+".png");
			}
		} catch (Exception e) {
			
		}
	}
}
