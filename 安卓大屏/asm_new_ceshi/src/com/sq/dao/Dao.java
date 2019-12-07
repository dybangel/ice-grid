package com.sq.dao;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.apache.http.client.HttpResponseException;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import android.content.Context;
import android.content.pm.PackageManager.NameNotFoundException;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import com.bjw.ComAssistant.PortService;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.sq.util.Config;
import com.sq.util.Util;

public class Dao {

	public String getKC(String mechineID,String productID,String dgOrderDetailID,Context context)
	{
		String MethodName = "getKC";
		String soapAction = Config.NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(Config.NameSpace, MethodName);
		request.addProperty("mechineID", mechineID);
		request.addProperty("productID", productID);
		request.addProperty("dgOrderDetailID", dgOrderDetailID);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(Config.url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				if("200".equals(result))
				{
					return result;
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	public String getReqsn(String companyID,String mechineID,String productID,String sftj,String product,String dgOrderDetailID,String type,Context context)
	{
		String MethodName = "getReqsn";
		String soapAction = Config.NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(Config.NameSpace, MethodName);
		request.addProperty("companyID", companyID);
		request.addProperty("mechineID", mechineID);
		request.addProperty("productID", productID);
		request.addProperty("sftj", sftj);
		request.addProperty("product", product);
		request.addProperty("dgOrderDetailID", dgOrderDetailID);
		request.addProperty("type", type);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(Config.url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				return result;
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	
	public String updateProductList(String mechineBH, Context context) {
		String MethodName = "getProductList2";
		String soapAction = Config.NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(Config.NameSpace, MethodName);
		request.addProperty("mechineID", Util.getSharePer(context, "mechineID"));
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(Config.url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				JsonParser parser=new JsonParser();  //创建JSON解析器
				JsonObject object=(JsonObject)parser.parse(result);
				String code=object.get("code").getAsString();
				if("200".equals(code))
				{
					String productInfo=object.get("db").getAsJsonArray().toString();
					Util.writeBillTxtToFile("productInfo:" + productInfo + "-->>>:");
					Log.i("1010","12"+productInfo);
					JSONArray jsonArray = new JSONArray(productInfo);
					List<JSONObject> list = new ArrayList<JSONObject>();
					for (int i = 0; i < jsonArray.length(); i++) {
						JSONObject jsonObject = jsonArray.getJSONObject(i);
						String name= jsonObject.getString("proName");
						String sftj= jsonObject.getString("sftj");
						 Log.i("5555","name="+name+";sftj="+sftj);
					}
					//Config.productlistno=object.getString("productlistno");
					Util.setSharePer(context, "productInfo", productInfo);
					Util.setSharePer(context, "priceSwitch", object.get("priceSwitch").getAsString());
				}else {
					Util.setSharePer(context, "productInfo", "");
					Util.setSharePer(context, "priceSwitch", "0");
				}
				// 产品信息=[{"productID":167,"proName":"新希望鲜牛奶（瓶装）","price1":5.0000,"price2":4.0000,"price3":2.0000,"path":"/img/20180502151405.jpg","protype":29,"mechineID":null,"description":"味道不错","productSize":null,"bzq":20,"companyID":13,"ljxs":3,"httpImageUrl":"http://nq.bingoseller.com/img/20180502151405.jpg","sluid":"1","progg":"1","num":10},{"productID":171,"proName":"产品五","price1":5.0000,"price2":4.0000,"price3":2.0000,"path":"/img/20180327134406.jpg","protype":21,"mechineID":null,"description":"味道不错","productSize":null,"bzq":20,"companyID":13,"ljxs":1,"httpImageUrl":"http://nq.bingoseller.com/img/20180327134406.jpg","sluid":null,"progg":"1","num":5},{"productID":172,"proName":"产品六","price1":5.0000,"price2":4.0000,"price3":2.0000,"path":"/img/20180327134414.jpg","protype":21,"mechineID":null,"description":"味道不错","productSize":null,"bzq":20,"companyID":13,"ljxs":null,"httpImageUrl":"http://nq.bingoseller.com/img/20180327134414.jpg","sluid":null,"progg":"1","num":5}]
			}
		} catch (Exception e) {
			Log.i("5555", "e="+e.getMessage());
			e.printStackTrace();
		}
		return result;
	}
	// 获取最新的视频列表
	public String updateVideoList(String mechineID, Context context) {
		// /
		String MethodName = "getVideoList";
		String soapAction = Config.NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(Config.NameSpace, MethodName);
		request.addProperty("mechineID", mechineID);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(Config.url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				Object response = (Object) envelope.getResponse();
				result = response.toString();
				if (!"1".equals(result) && result != null && !"".equals(result)) {
					JSONArray jsonArray = new JSONArray(result);
					Util.setSharePer(context, "videoInfo", result);
					// 视频信息=[{"ID":164,"mechineID":24,"videoID":140,"type":0,"bz":"","times":1124,"tfTime":"2018-09-28 14:32:28","tfType":"2","tfcs":0,"valiDate":"2025-09-28 00:00:00","zt":"0","path":"http://nq.bingoseller.com/main/Advertisement/upload/hvideo/20180928142946.mp4","description":"横屏","name":"20180928142946.mp4"}]
				} else {
					Util.setSharePer(context, "videoInfo", "");
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	private static String url = "http://nq.bingoseller.com/api/mechineService.asmx";
	private static String NameSpace = "http://nq.bingoseller.com/";
	 
	/***
	 * 更新产品类别
	 * @param context
	 */
	public void updateProductType(Context context) {
		String MethodName = "getProductType";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("mechineID", Util.getSharePer(context, "mechineID"));
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				if (!"1".equals(result)&&result!=null&&!"".equals(result)) {
					JSONArray jsonArray = new JSONArray(result);
					Util.setSharePer(context, "productType", result);
				}else {
					Util.setSharePer(context, "productType", "");
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	/**
	 * 上传产品销售记录
	 * @param recordList
	 * [{"payType":2,"productID":256,"orderTime":"2019-02-25 16:11:35",
	 * "num":1,"totalMoney":2.9,"proLD":"26","type":"2",
	 * "orderNO":"1551082295897","bz":"交易成功",
	 * "billno":"111973610000624640","mechineID":"34"}]
	 * @return
	 */
	public static String upSellRecordList(String recordList,Context context) {
		String MethodName = "upSellRecord";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("recordList", recordList);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
			}
		} catch (Exception e) {
			JSONObject reqJson = new JSONObject();
			try {
				reqJson.put(Config.CMD, "upSellRecordList");
				reqJson.put(Config.MECHINE_ID, Util.getSharePer(context, "mechineID"));
				reqJson.put("MsgId", "");
				reqJson.put("recordList", recordList);
				reqJson.put("samtype", PortService.Getjiqigezhongxinxi());
				Util.sendMsg(reqJson.toString());

			} catch (JSONException ex) {
				e.printStackTrace();
			}
			e.printStackTrace();
		}
		
		return result;
	}
	/***
	 * 上传地理位置信息
	 * @param str
	 * @return
	 */
	public String upLoadLocation(String str) {
		String MethodName = "upLoadLocation";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("str", str);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				if ("1".equals(result)) {

				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	public static String dgCh(String code,String mechineID) {
		String MethodName = "dgCh";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("code", code);
		request.addProperty("mechineID", mechineID);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	//获取温度
	public static String readZTMechine(String mechineID,String temperature,Context context) {
		String MethodName = "readZTMechine2";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("mechineID", mechineID);
		request.addProperty("temperature", temperature);
		request.addProperty("versions", getVerName(context));
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				
				if (!"1".equals(result)) {
					Util.setSharePer(context, "mechineInfo", result);
				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
	public static String getVerName(Context context) {
		String verName = "";
		try {
			verName = context.getPackageManager().getPackageInfo(
					"com.bjw.ComAssistant", 0).versionName;
		} catch (NameNotFoundException e) {
		}
		return verName;
	}
	/***
	 * 
	 * @param payType 1微信2支付宝3微信公众号 4余额
	 * @param productID 产品ID
	 * @param totalMoney 付款总额
	 * @param proLD 料道号
	 * @param type 1订购 2零售
	 * @param billno 第三方支付流水号
	 * @param mechineID 机器ID
	 * @param bz 备注信息
	 * @param memberID 会员ID
	 * @return
	 */
	
	//[{"productID":"282","orderTime":"2019-03-22 10:06:26","num":1,"proLD":"20","type":"1","orderNO":1553220386205,"bz":"交易成功","billno":1553220386205,"mechineID":"25","memberID":"449"}]
	//[{"memberID":"222","code":"422250","payType":0,"productID":274,"orderTime":"2019-03-16 19:55:06","num":1,"totalMoney":4,"proLD":"55","type":"1","orderNO":"1552737306789","bz":"交易成功","billno":"1552737282168","mechineID":"33"}]
	public static  String returnJson(String payType,String productID,String totalMoney,String proLD,String type,String billno,String mechineID,String bz,String memberID,String code)
	{
		SimpleDateFormat sf=new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		JSONObject reqJson = new JSONObject();
		try {
			reqJson.put("payType", payType);
			reqJson.put("productID", productID);
			reqJson.put("orderTime", sf.format(new Date()));
			reqJson.put("num", 1);
			reqJson.put("totalMoney", totalMoney);
			reqJson.put("proLD", proLD);
			reqJson.put("type", type);
			reqJson.put("orderNO", System.currentTimeMillis());
			reqJson.put("bz", bz);
			if(code==null)
			{
				reqJson.put("code", "");
			}else {
				reqJson.put("code", code);
			}
			
			if("".equals(billno)||billno==null)
			{
				reqJson.put("billno", System.currentTimeMillis());
			}else {
				reqJson.put("billno", billno);
			}
			
			reqJson.put("mechineID", mechineID);
			reqJson.put("memberID", memberID);
			return "["+reqJson+"]";
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return null;
	}
	public static String updatePayInfo(String trxid) {
		String MethodName = "updatePayInfo";
		String soapAction = NameSpace + MethodName;
		String result = "";
		SoapObject request = new SoapObject(NameSpace, MethodName);
		request.addProperty("trxid", trxid);
		SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(
				SoapEnvelope.VER11);
		envelope.dotNet = true;
		envelope.setOutputSoapObject(request);
		HttpTransportSE ht = new HttpTransportSE(url);
		try {
			ht.call(soapAction, envelope);
		} catch (HttpResponseException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} catch (XmlPullParserException e) {
			e.printStackTrace();
		}
		try {
			if ((Object) envelope.getResponse() != null) {
				SoapPrimitive response = (SoapPrimitive) envelope.getResponse();
				result = response.toString();
				if ("1".equals(result)) {

				}
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		return result;
	}
 
}
