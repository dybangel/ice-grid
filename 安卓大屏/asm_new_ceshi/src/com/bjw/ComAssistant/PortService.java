package com.bjw.ComAssistant;

import java.io.IOException;
import java.security.InvalidParameterException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.Locale;
import java.util.Queue;

import net.sf.json.util.NewBeanInstanceStrategy;

import org.json.JSONException;
import org.json.JSONObject;

import android.R.bool;
import android.R.integer;
import android.R.string;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager.NameNotFoundException;
import android.os.Binder;
import android.os.Handler;
import android.os.IBinder;
import android.os.Message;
import android.os.Parcel;
import android.os.RemoteException;
import android.util.Log;
import android.widget.TextView;

import com.bjw.bean.ComBean;
import com.sq.dao.Dao;
import com.sq.util.Config;
import com.sq.util.Util;

public class PortService extends Service {
	public static String com_A = "/dev/ttyS1";
	public static String comA_rate = "9600";
	public static SerialControl ComA;//
	public static DispQueueThread DispQueue;// ˢ����ʾ�߳�
	public static String recLogString;
	public static String chLD = "";// �����ϵ�
	public static boolean is_ch = false;// �Ƿ����
	public static boolean is_search_tem = false;
	
	public static int temperature = 0;// �¶���Ϣ
	public String mechineID = "";
	public static int STATUS = 1;// 0��æ״̬ 1æ״̬�����Գ���
	public static String payType = "";// ֧������ 1΢��2֧���� 3΢�Ź��ں�4���
	public static String billno = "";// ������֧���ĵ���
	public static String orderType;// 1����2 ���� 0���
	public static boolean chRecord = false;// ������¼���� �ύһ���ó�false
	public static String productID = "";
	public static String money = "0";
	public static String memberID = "0";
	public static String code = "";// ȡ����
	public static Date sclxsj = new Date();// �ϴ���ѯ�����ʱ�� �տ���ʱ�� ���������ʱ��
	public static boolean chjs = true;// �Ƿ����
	public static Date jqmtime = new Date();// ��������æ��ʱ��
	public static int comlianjie = 0;// com �������� �յ���Ϣ��ֵ1 ���ͷ�������ֵ0
	public static Date jrlxsj = new Date();// ������ѯʱ��
	public static int ld = 9;// ������ѯʱ��
	public static boolean opendoor =false;
	public static Date opendoortime = new Date();// �ϴ���ѯ�����ʱ�� �տ���ʱ�� ���������ʱ��

	public static List<String> stlist = new ArrayList();
	public static boolean is_return_tem = true;//��ѯ�¶��Ƿ��л�ֵ��true�ѻظ�
	
	
	
	
	//�¼ӵ���������Ϣ
	
	public static String productListNo="";// ��Ʒ�б���
	public static String priceSwitch="";// �Ƿ�����Ա��
	
	public static String productTypeNo="";// ��Ʒ���ͱ��
	public static String videoListNo="";// ��Ƶ�б���
	public static String mechineOtherStatus="";// ������������Ϣ
	
	public static boolean isSetTime =false;//�������趨ʱ��
	public static boolean chEnd = true;// �Ƿ������ɣ�����ѯʱ���㲥���ز�Ʒҳ�棬����״̬������false��Ȼ����������ɹ���ʧ�ܺ�fasocket�������������������������chEndָ�׿���յ��󽫴˸���true
	
	
	public static void csch()

	{
		int fenzhong1 = (int) ((new Date().getTime() - jrlxsj.getTime()) / (1000));
		Util.writeBillTxtToFile("��ѯ:" + fenzhong1 + "s"
				+ (STATUS == 0 ? "���У����Խ���ָ��" : "����δ����"));

		if (STATUS == 0 && fenzhong1 > 5) {

			if (ld % 10 == 7 && ((int) (ld + 10) / 10) == 5) {
				ld = 10;
			} else if (ld % 10 == 7) {
				ld = ((int) (ld + 10) / 10) * 10;

			} else {

				ld = ld + 1;
			}
			//������¶Ȳ�ѯ����ִ�в�ѯ
			SimpleDateFormat sfDateFormat1 = new SimpleDateFormat("MMddHHmmss");

			chLD = ld + "";
			is_ch = true;
			payType = "1";
			billno = sfDateFormat1.format(new Date());
			orderType = "";
			productID = "";
			money = "" + 1;
			memberID = "" + 1;// ������ʱ���������ֶ�
			code = "222";
			Util.debuglog("���ţ�"+Config.billno+"�ϵ���"+ld+"��ʼ","chlcAll");
			Util.writeBillTxtToFile("׼������:" + billno + "�ϵ���" + chLD + "-->>>:");
			Config.billno = billno;

			Config.billno = billno;

			Config.type = 2;

		}

	}

	public static void sendchlc(String chlcString) {
		SimpleDateFormat sfDateFormat1 = new SimpleDateFormat("hh:mm:ss SSS");

		stlist.add(sfDateFormat1.format(new Date()) + "-----|  "
				+ Config.billno + "����," + ld + "�ϵ�" + chlcString);
		if (stlist.size() > 30) {
			stlist.remove(0);

		}
		StringBuilder S = new StringBuilder();
		for (String item : stlist) {

			S.append(item + "\r\n");

		}

		Message msg = new Message();

		msg.obj = S.toString();
		IndexActivity.chlchandler.sendMessage(msg);

	}
	public static String Getjiqigezhongxinxi() {
		JSONObject reqJson= new JSONObject();
		try {
			int openDoorTime=0; 
			//ȡ���ڴ�
			if(opendoor){
				openDoorTime = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
					//���͵�ǰ������״̬������������ʱ��
			}
			//������δ�������������ѯһ���¶�
			reqJson.put("TEXT", "�豸��һЩ״̬");
			int fenzhong = (int) ((new Date().getTime() - sclxsj.getTime()) / (1000 * 60));
			reqJson.put("t1", fenzhong);// ��һ����ѯ �����ʱ��� ����
			reqJson.put("t2", temperature);// �¶�
			reqJson.put("t3", comlianjie);// com������
			reqJson.put("t4", STATUS);// ��������״̬
			reqJson.put("t5", openDoorTime);// �����Ŵ򿪵�ʱ��
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	public static String Getjiqigezhongxinxi(JSONObject reqJson) {
		if(reqJson==null){
			reqJson= new JSONObject();
		}
		try {
			int openDoorTime=0; 
			//ȡ���ڴ�
			if(opendoor){
				openDoorTime = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
				
					
					//���͵�ǰ������״̬������������ʱ��
				
				
			}
			
			
			//������δ�������������ѯһ���¶�
			int fenzhong = (int) ((new Date().getTime() - sclxsj.getTime()) / (1000 * 60));
			reqJson.put("lastLXTime", fenzhong);// ��һ����ѯ �����ʱ��� ����
			reqJson.put("temperature", temperature);// �¶�
			//reqJson.put("comlianjie", comlianjie);// 
			reqJson.put("mechineStatus", STATUS);// ��ѯû������ʱ��0��������1
			reqJson.put("busyStatus", Config.busy);//�Ƿ��ڳ���
			reqJson.put("openDoorTime", openDoorTime);// �����Ŵ򿪵�ʱ��
			reqJson.put("mechineOtherStatus", mechineOtherStatus);// �����б���
			
			reqJson.put("gkjStatus", Config.gkjStatus);// ���ػ�״̬
			reqJson.put("productListNo", productListNo);// ��Ʒ�б���
			reqJson.put("priceSwitch", priceSwitch);// �Ƿ�����Աģʽ
			reqJson.put("videoListNo", videoListNo);// ��Ƶ���
			
			reqJson.put("productTypeNo", productTypeNo);// �����б���
			reqJson.put("netWorkType", Config.netWorkType);// ��������
			reqJson.put("netLevel", Config.netLevel);// ����ǿ��
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	public static String GetBillObject() {
		JSONObject reqJson= new JSONObject();
		try {
			reqJson.put("ldNO",chLD);
			reqJson.put("payType",payType);
			reqJson.put("billno",billno);
			reqJson.put("productID",productID);
			reqJson.put("money",money);
			reqJson.put("type",orderType);
			reqJson.put("memberID",memberID);
			reqJson.put("code",code);
			return reqJson.toString();

		} catch (Exception e) {
			return reqJson.toString();
		}

	}
	@Override
	public IBinder onBind(Intent intent) {
		return null;
	}

	@Override
	public int onStartCommand(Intent intent, int flags, int startId) {
		try {
			chLD = intent.getStringExtra("ldNO");
			is_ch = intent.getBooleanExtra("is_ch", false);
			payType = intent.getStringExtra("payType");
			billno = intent.getStringExtra("billno");
			orderType = intent.getStringExtra("orderType");
			productID = intent.getStringExtra("productID");
			money = intent.getStringExtra("money");
			memberID = intent.getStringExtra("memberID");// ������ʱ���������ֶ�
			code = intent.getStringExtra("code");
			
		} catch (Exception e) {
			Log.i("4444", "onStartCommand=" + e.getMessage() + "��ֵ����");
		}
		return super.onStartCommand(intent, flags, startId);
	}

	@Override
	public void onCreate() {
		mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
		open();
		
		super.onCreate();
	}

	public void open() {
		ComA = new SerialControl();
		DispQueue = new DispQueueThread();
		DispQueue.start();
		ComA.setPort(com_A);
		ComA.setBaudRate(comA_rate);
		OpenComPort(ComA);
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
	}

	private static class SerialControl extends SerialHelper {
		public SerialControl(String sPort, String sBaudRate) {
			super(sPort, sBaudRate);
		}

		public SerialControl() {

		}

		@Override
		protected void onDataReceived(final ComBean ComRecData) {
			DispQueue.AddQueue(ComRecData);// �̶߳�ʱˢ����ʾ(�Ƽ�)
		} 
	}

	// ----------------------------------------------------ˢ����ʾ�߳�
	private class DispQueueThread extends Thread {
		private Queue<ComBean> QueueList = new LinkedList<ComBean>();

		@Override
		public void run() {
			super.run();
			while (!isInterrupted()) {
				final ComBean ComData;
				while ((ComData = QueueList.poll()) != null) {
					try {
						DispRecData(ComData);
						// Thread.sleep(100);// ��ʾ���ܸߵĻ������԰Ѵ���ֵ��С������̫�߻���ɲ�����
					} catch (Exception e) {
						e.printStackTrace();
					}
					break;
				}
			}
		}

		public synchronized void AddQueue(ComBean ComData) {
			QueueList.add(ComData);
		}
	}

	// ----------------------------------------------------��ʾ��������
	public static String REC_DATA = "";

	private void DispRecData(ComBean ComRecData) {
		StringBuilder sMsg = new StringBuilder();
		sMsg.append(ComRecData.sRecTime);
		sMsg.append("[");
		sMsg.append(ComRecData.sComPort);
		sMsg.append("]");
		sMsg.append("[Hex] ");
		sMsg.append(MyFunc.ByteArrToHex(ComRecData.bRec));
		sMsg.append("\r\n");
		String recData = MyFunc.ByteArrToHex(ComRecData.bRec).replaceAll(" ",
				"");

		if (recData.length() >= 2 && "FE".equals(recData.substring(0, 2))) {
			REC_DATA = recData;
		} else {
			REC_DATA += recData;
		}
		if (REC_DATA.length() > 10) {
			String jyh = makeChecksum(REC_DATA.substring(0,
					REC_DATA.length() - 2));
			if (jyh.equals(REC_DATA.substring(REC_DATA.length() - 2,
					REC_DATA.length()))) {
				init(REC_DATA);
				final Intent intent = new Intent();
				intent.setAction(IndexActivity.PORT_ACTION);
				intent.putExtra("status", STATUS);
				// intent.putExtra("hex", REC_DATA);
				sendBroadcast(intent);
			}
		}
	}

	// ----------------------------------------------------�򿪴���
	public void OpenComPort(SerialHelper ComPort) {
		try {
			ComPort.open();
		} catch (SecurityException e) {
			ShowMessage("�򿪴���ʧ��:û�д��ڶ�/дȨ��!");
		} catch (IOException e) {
			ShowMessage("�򿪴���ʧ��:δ֪����!");
		} catch (InvalidParameterException e) {
			ShowMessage("�򿪴���ʧ��:��������!");
		}
	}

	public String bzInfo = "";
	//�ϵ�״̬�������쳣1��������ѯʱ��Ϊ0
	
	public static String reshex(String hex) {
		
		
		if (hex.contains("FE55EF1278")) {
			return "������λ��ʼ��";
		} else if (hex.contains("FE55EF0D73")) {
			return "�жϻ�������";
		} else if (hex.contains("FE55EF1573")) {
			return "��ȡ�ϵ�������Ϣ";
		} else if (hex.contains("FE55EF2C7D")) {
			return "ϵͳ����  ״̬��Ϣ";
		} else if (hex.contains("FE55EF2F7C")) {
			return "������Ϣ  ������Ϣ";
		} else if (hex.contains("FE55EF137A")) {
			return "�ϵ�����״̬";
		} else if (hex.contains("FE55EF0979")) {
			return "ϵͳ����״̬";
		} else if (hex.contains("FE55EF0D7B")) {
			return "�ϵ����޻���Ϣ";
		}
		// 76
		else if (hex.contains("FE55EF0376F6")) {
			return "������û׼����";
		} 
		else if (hex.contains("FE55EF0376F6")) {
			return "������û׼����";
		} else if (hex.contains("FE55EF0376F8")) {
			return "�޷�����������";
		} else if (hex.contains("FE55EF0376F5")) {
			return "�������к���ͬ";
		} else if (hex.contains("FE55EF037600")) {
			return "�������";
		} else if (hex.contains("FE55EF11760700000000000000000000000000008E")) {
			return "�������������Խ����µ�ָ��";
		} else if (hex.contains("FE55EF11760700000000000000000000000000018F")) {
			return "ȡ����δ�� ����û�и�λ";
		} else if (hex.contains("FE55EF117607000000000000000000000000000593")) {
			return "�п���������û�й�";
		} else if (hex.contains("FE55EF117607000000000000000000000000000492")) {
			return "�п�����������û�й�";
		}

		else if (hex.contains("FE55EF117601")) {
			return "�����У�������æ";
		}

		else if (hex.contains("FE55EF037601".trim())) {
			return "����æ";
		} else if (hex.contains("FE55EF0376FF".trim())) {
			return "����ʧ��";
		} else if (hex.contains("FE55EF0376FA".trim())) {
			return "��ʱ����";
		} else if (hex.contains("FE55EF0376FE".trim())) {
			return "У�����";
		} else if (hex.contains("FE55EF0376F4".trim())) {
			return "�ϵ�����";
		} else if (hex.equals("FE55EF0376F3".trim())) {
			return "�ϵ�����";
		}

		return "δ֪����";

	}

	public static String sendhex(String hex) {

		if (hex.equals("EF55FE0378007B")) {
			return "��λָ��";
		} else if (hex.equals("EF55FE0676000000007C")) {
			return "��ѯָ��";
		} else if (hex.equals("EF55FE067673000000EF".trim())) {
			return "�� ѯ �� �� �� �� ��";
		} else if (hex.equals("EF55FE03730076".trim())) {
			return "�ϵ�����Ӧ��";
		} else if (hex.equals("EF55FE067673010000F0".trim())) {
			return "��ѯ���ɻ�����";
		}

		else if (hex.contains("EF55FE06767D000000F9".trim())) {
			return "״̬��ѯ";
		} else if (hex.contains("EF55FE037D0080".trim())) {
			return "״̬Ӧ��";
		} else if (hex.contains("EF55FE0379007C".trim())) {
			return "ϵͳ����״̬";
		} else if (hex.contains("EF55FE06767A000000F6".trim())) {
			return "�ϵ����ϖ�ѯ";
		} else if (hex.contains("EF55FE037B007E".trim())) {
			return "�ϵ���Ϣ��ʼ��";
		} else if (hex.contains("EF55FE037A007D".trim())) {
			return "���ϻظ�";
		} else if (hex.contains("EF55FE067677000000F3".trim())) {
			return "������ѯ";
		}

		else if (hex.contains("EF55FE0377007A".trim())) {
			return "������ѯ";
		} else if (hex.contains("EF55FE067677000000F3".trim())) {
			return "������ѯ�ظ�";
		} else if (hex.contains("EF55FE177605".trim())) {
			return "����ָ��";
		} else if (hex.contains("EF55FE037C007F".trim())) {
			return "��������";
		} else if (hex.contains("EF55FE097604".trim())){
			return "�趨ʱ��";
			
		}

		return "û�ж�Ӧ����";
	}
	//����������Ϣ
	public boolean baseInit(String str) {
		
		if (str.trim().length() <= 10)
			return true;
		//������λ��ʼ��
		if (str.trim().substring(0, 10).equals("FE55EF1278")) {
			sendPortData("EF55FE0378007B");
			return true;
		}
		//�жϻ�������
		if (str.trim().substring(0, 10).equals("FE55EF0D73")) {// �ϵ�����
			sendPortData("EF55FE03730076");
			return true;
		}
		//��ȡ�ϵ�������Ϣ
		if (str.trim().substring(0, 10).equals("FE55EF1573")) {
			sendPortData("EF55FE03730076");
			return true;
		}
		//ϵͳ����  ״̬��Ϣ
		if (str.trim().substring(0, 10).equals("FE55EF2C7D")) {
			// ϵͳ���� ״̬��Ϣ
			
			if (str.length() >= 96) {
				// ��ȡ�¶���Ϣ
				temperature = Integer.parseInt(Util.getTemper(str.substring(84,
						86)));
				is_return_tem = true;
			}
			sendPortData("EF55FE037D0080");
			return true;
		}
		//ϵͳ����״̬
		if (str.trim().substring(0, 10).equals("FE55EF0979")) {// ϵͳ����״̬
			
			sendPortData("EF55FE0379007C");
			return true;
		}
		//�ϵ�����״̬
		if (str.trim().substring(0, 10).equals("FE55EF137A")) {// �ϵ�����״̬
			sendPortData("EF55FE037A007D");
			return true;
		}
		//�ϵ����޻���Ϣ
		if (str.trim().substring(0, 10).equals("FE55EF0D7B")) {// �ϵ����޻���Ϣ
			sendPortData("EF55FE037B007E");
			return true;
		}
		return false;
		
	}
	public void init(String str) {
		Config.lastgkjtime=new Date();
		Config.gkjStatus = 0;
		Util.writeBillTxtToFile("����:" + reshex(str) + "<<<-:" + str);
		//csch();
		
		if(baseInit(str)){
			return;
			
		};
		if (str.equals("FE55EF11760700000000000000000000000000018F")) 
		{
			//ȡ���ڴ�
			if(!opendoor){
				
				opendoor=true;//����״̬
				opendoortime=new Date();							
				Util.debuglog("���ţ�"+Config.billno+"�ϵ���"+ld+"��ȡ����","chlcAll");
			}
			int opendoortimes = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
			if(opendoortimes>60&&opendoortimes%60==0){
				
				//���͵�ǰ������״̬������������ʱ��
			}
			//ȡ���ſ���ʱ��
			
		}else {
			if(opendoor){
				int opendoortimes = (int) ((new Date().getTime() - opendoortime.getTime()) / (1000));
				opendoor=false;//���Źر�
				Util.debuglog("���ţ�"+Config.billno+"�ϵ���"+ld+"ȡ���Źر� �����ܼ���ʱ��"+opendoortimes+"s","chlcAll");
			}
		}
		
		// �����ǰ����ѯ��8E�������г������񣬷������񲢸ı�����״̬Ϊ�ѷ��͸���λ��
		// ����ָ��-->>>:EF55FE177605006201110100000000000001000100000000000009
		// ����æ<<<-:FE55EF0376017A ��������Ѿ��յ�ָ�� ��ʼ�ɻ�
		// FE55EF117601 ������
		// FE55EF03760079 �������
		// FE55EF2F7C0111 ���
		//is_ch = false;
		
		if (str.trim().substring(0, 12).equals("FE55EF117607")) {// ��ѯ
			// �жϺ���λ��8E ����������ѯ ������ǻ���Ҫô��ͣӪҵҪô��û�غ�
			sclxsj=new Date();
			if (!str.trim().substring(str.trim().length() - 2, str.length())
					.equals("8E")) {
				STATUS = 1;
				return;
			}
			if (is_ch) {
				
				STATUS = 1;
				Integer ld = Integer.parseInt(chLD);
				String hexld = ld.toHexString(ld);
				String xlh = Util.getXLH();
				String str1 = "EF55FE17760500"
						+ xlh
						+ "01"
						+ ld
						+ "01000000000000010001000000000000"
								.replaceAll(" ", "");
				sendPortData(str1 + makeChecksum(str1));
				is_ch = false;
				chRecord = true;
				final Intent intent = new Intent();
				intent.setAction(Config.ACTION_JUMP_URL);
				intent.putExtra("TYPE", "1"); // type=1 ��ת������ʱҳ��
				sendBroadcast(intent);
				
			} else {
				if (is_search_tem) {
					STATUS = 1;
					
					sendPortData("EF55FE06767D000000F9");// ��ѯ����״̬
					Log.i("1111","EF55FE06767D000000F9");
					is_search_tem = false;
					final Intent intent = new Intent();
					intent.setAction(MainActivity.ACTION_UPDATEUI);
					intent.putExtra("temperature", temperature);
					sendBroadcast(intent);
					return;
				}else if(!isSetTime){
					//����ʱ������������ʱ��
					//˵��������������ʱ��ֻ�ڿ�������յ���һ����������ѯʱ���ã�֮��Ͳ�����
					//����ķ��������ѧϰ��
					//ǰ��EF55FE�����㣬�����2λ��һλ��10����ת16����д�������һλ��У��ͣ���ǰ��ļ������10���Ƽ�����Ȼ����д��16����
					//16����1-9+ABCDEFG��ʮ��λ
					//���ӣ������ǽ���ǰʱ��д��16λ��������ȥ����λ
					STATUS = 1;
					Calendar now = Calendar.getInstance();
					int year= now.get(Calendar.YEAR);
					int month=now.get(Calendar.MONTH) + 1;
					int day=now.get(Calendar.DAY_OF_MONTH) ;
					int hour=now.get(Calendar.HOUR_OF_DAY);
					int min=now.get(Calendar.MINUTE) ;
					int second=now.get(Calendar.SECOND);
					int total=131+(year%100)+month+day+hour+min+second;
					int year1=(year%100)/16;
					int year2=(year%100)%16;
					int month1=month/16;
					int month2=month%16;
					int day1=day/16;
					int day2=day%16;
					int hour1=hour/16;
					int hour2=hour%16;
					int min1=min/16;
					int min2=min%16;
					int second1=second/16;
					int second2=second%16;
					int total1=total/16;
					int total2=total%16;
					String setTimeString=tenToSixTeen(year1)+tenToSixTeen(year2)
							+tenToSixTeen(month1)+tenToSixTeen(month2)
							+tenToSixTeen(day1)+tenToSixTeen(day2)
							+tenToSixTeen(hour1)+tenToSixTeen(hour2)
							+tenToSixTeen(min1)+tenToSixTeen(min2)
							+tenToSixTeen(second1)+tenToSixTeen(second2)
							+tenToSixTeen(total1)+tenToSixTeen(total2);
					sendPortData("EF55FE097604"+setTimeString);// ��ѯ����״̬
					Util.debuglog("ʱ�䣺"+year+"-"+month+"-"+day+" "+hour+":"+min+":"+second+"����:"+"EF55FE097604"+setTimeString,"setTime");
					
					isSetTime=true;
					
				}
				else {
					// ������ѯ
					STATUS = 0;
					chRecord = false;
					if(chEnd){
						Log.i("1111","chEnd");
						final Intent intent = new Intent();
						intent.setAction(Config.ACTION_JUMP_URL);
						intent.putExtra("TYPE", "2"); // type=1 ��ת������ʱҳ��
														// type=2 ��ת����Ʒ�б�ҳ��
						sendBroadcast(intent);
						chEnd=false;
					}
					
					Config.busy=0;
					sendPortData("EF55FE037C007F");
				}
			}
			
		}
		
		//���ڳ���
		if (str.trim().substring(0, 12).equals("FE55EF117601")) {
			Config.busy = 1;
			try {
				JSONObject jsonObject= new JSONObject();
				jsonObject.put(Config.MECHINE_ID, mechineID);
				jsonObject.put("cmd", "ch");
				jsonObject.put("SendMsg", GetBillObject());
				jsonObject.put("MsgId", "");
				JSONObject samtypeJson= new JSONObject();
				samtypeJson.put(Config.CMD, "chAlways");
				jsonObject.put("samtype", Getjiqigezhongxinxi(samtypeJson));
				Util.sendMsg(jsonObject.toString());
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			sendPortData("EF55FE0676000000007C");
			return;
		}
		//�ϵ�����ģ�����û���ҵ�ƥ��Ļ�ֵ
		//���Ұ��ϵ�����д
		if (str.equals("FE55EF0376F36C"))//�ϵ�����
		{
			if (chRecord) {
				chRecord = false;
				// �ύ������¼
				String json = "";
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "�ϵ�����";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				// �ύ������¼
				sendchFail(json);
				

			}

			bzInfo = "�ϵ�����";
			sendPortData("EF55FE0676000000007C");

			Util.debuglog("���ţ�"+Config.billno+"�ϵ���"+chLD+"�ϵ�����","chlcAll");
			return;
		}
		
		
		//�ɹ���ʧ��
		if (str.trim().substring(0, 10).equals("FE55EF2F7C")) {// ������Ϣ ������Ϣ
			recLogString = "������Ϣ  ������Ϣ";
			// �˴���ѯ�����Ƿ�ɹ� ÿ����һ������vmc����������һ�� ���vmcû���յ���Ӧ���ڷ���һ����෢��3��
			// ��Ҫ�˶Խ������к�
			if (str.trim().length() == 102) {
				String xlh = str.substring(86, 88);
				String ldNO = str.substring(12, 14);
				String status = str.substring(60, 62);// 00�����׳ɹ�01������ʧ��
				if ("00".equals(status)) {
					// ���׳ɹ�
					if (chRecord) {
						chRecord = false;
						
						String json = "";
						String pay_Type = payType;
						String product_ID = productID;
						String totalMoney = money;
						String proLD = chLD;
						String type = orderType;
						String bill_no = Config.billno;
						String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
						String bz = "���׳ɹ�";
						json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
						
						sendchSuccess(json);
						
						
						
					}
				} else if ("01".equals(status)) {
					if (chRecord) {
						chRecord = false;
						
						String json = "";
						String pay_Type = payType;
						String product_ID = productID;
						String totalMoney = money;
						String proLD = chLD;
						String type = orderType;
						String bill_no = Config.billno;
						String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
						String bz = "����ʧ��";
						json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);

						sendchFail(json);
						
					
						
					}
				}
			}
			//��������
			sendPortData("EF55FE037C007F");
			return;
		}
		// ��������
		if (str.trim().substring(0, 12).equals("FE55EF117602")) {
			Config.busy = 0;
			// �ϵ�û����
			JSONObject reqJson = new JSONObject();
			if (chRecord) {
				chRecord = false;
				// �ύ������¼
				String json = "";
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "����ʧ��";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				// �ύ������¼
				
				sendchFail(json);
			}
			
				

		

			

			sendPortData("EF55FE0676000000007C");
			// sendPortData("EF55FE037C007F");
			return;
		}
		if (str.trim().substring(0, 12).equals("FE55EF117604")) {
			Config.busy = 0;
			STATUS = 1;
			String json = "";
			// ���׳ɹ�
			if (chRecord) {
				chRecord = false;
				// �ύ������¼
				String pay_Type = payType;
				String product_ID = productID;
				String totalMoney = money;
				String proLD = chLD;
				String type = orderType;
				String bill_no = Config.billno;
				String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
				String bz = "���׳ɹ�";
				json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
				sendchSuccess(json);
				

			}
			// sendPortData("EF55FE0676000000007C");
			sendPortData("EF55FE037C007F");
			return;
		}
		if (str.trim().substring(0, 10).equals("FE55EF0376")) {
			Config.busy = 0;
			STATUS = 1;

			// 20191029 03:26:24 030:����:���׳ɹ�<<<-:FE55EF03760079
			if (str.trim().substring(0, 12).equals("FE55EF037600")) {// ���׳ɹ�
				bzInfo = "���׳ɹ�";
				Log.i("1111","FE55EF037600");
				sendPortData("EF55FE0676000000007C");
				return;
			}
			//����æ
			if (str.trim().substring(0, 12).equals("FE55EF037601")) {//
				Config.busy = 1;
				
				try {
					JSONObject jsonObject= new JSONObject();
					jsonObject.put(Config.MECHINE_ID, mechineID);
					jsonObject.put("cmd", "ch");
					jsonObject.put("SendMsg", GetBillObject());
					jsonObject.put("MsgId", "");
					JSONObject samtypeJson= new JSONObject();
					samtypeJson.put(Config.CMD, "chBusy");
					jsonObject.put("samtype", Getjiqigezhongxinxi(samtypeJson));
					Util.sendMsg(jsonObject.toString());
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FF")) {// ����ʧ��

				if (chRecord) {
					chRecord = false;
					// �ύ������¼
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "����ʧ��";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// �ύ������¼
					
					sendchFail(json);
				}

				bzInfo = "����ʧ��";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FA")) {// ��ʱ ����
				recLogString = "��ʱ����";
				sendPortData("EF55FE0676000000007C");
				
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376FE")) {// У�����

				bzInfo = "У�����";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F4")) {// �ϵ����� //

				if (chRecord) {
					chRecord = false;
					// �ύ������¼
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "�ϵ�����";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// �ύ������¼
					sendchFail(json);
					
				}

				bzInfo = "�ϵ�����";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F3")) {// �ϵ�����

				if (chRecord) {
					chRecord = false;
					// �ύ������¼
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "�ϵ�����";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// �ύ������¼
					sendchFail(json);
				
				}

				bzInfo = "�ϵ�����";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F6")) {// ������û׼����
				recLogString = "������û׼����";

				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F8")) {// �޷�����������
				recLogString = "�޷�����������";

				sendPortData("EF55FE0676000000007C");
				return;
			}
			if (str.trim().substring(0, 12).equals("FE55EF0376F5")) {// �������к���ͬ
				recLogString = "�������к���ͬ";

				if (chRecord) {
					chRecord = false;
					
					// �ύ������¼
					String json = "";
					String pay_Type = payType;
					String product_ID = productID;
					String totalMoney = money;
					String proLD = chLD;
					String type = orderType;
					String bill_no = Config.billno;
					String mechineID = Util.getSharePer(getApplicationContext(), "mechineID");
					String bz = "�������к���ͬ";
					json = Dao.returnJson(pay_Type, product_ID, totalMoney,proLD, type, bill_no, mechineID, bz, memberID,code);
					// �ύ������¼
					sendchFail(json);
					
				}

				bzInfo = "�������к���ͬ";
				sendPortData("EF55FE0676000000007C");
				return;
			}
			return;
		}
		
		
		Log.i("kkk", str);
		// Util.sendLog(getApplicationContext(), str);
	}

	public static String makeChecksum(String data) {
		if (data.length() > 6) {
			data = data.substring(6, data.length());
			if (data == null || data.equals("")) {
				return "00";
			}
			int total = 0;
			int len = data.length();
			int num = 0;
			while (num < len) {
				String s = data.substring(num, num + 2);
				total += Integer.parseInt(s, 16);
				num = num + 2;
			}
			String str = ("" + total);
			str = Integer.toHexString(Integer.parseInt(str));
			if (str.length() == 1) {
				str = "0" + str;
			}
			return lowerToUpper(str.substring(str.length() - 2, str.length()));
		} else {
			return "00";
		}
	}

	public static String lowerToUpper(String str) {
		char[] ch = str.toCharArray();
		for (int i = 0; i < ch.length; i++) {
			if (((int) ch[i] > 96) && ((int) ch[i] < 123)) {
				ch[i] = (char) ((int) ch[i] - 32);
			}
		}
		String childStr = String.valueOf(ch);
		return childStr;
	}

	public static void sendPortData(String sOut) {
		if (ComA != null && ComA.isOpen()) {
			Util.writeBillTxtToFile("����:" + sendhex(sOut) + "-->>>:" + sOut);
			ComA.sendHex(sOut);
		}
	}

	// ------------------------------------------��ʾ��Ϣ
	private void ShowMessage(String sMsg) {
		Intent intent = new Intent();
		intent.putExtra("toast", "" + sMsg);
		intent.setAction("com.tools.ui.toast");
		sendBroadcast(intent);
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

	
	private void sendchFail(String json) {
		try {
			JSONObject jsonObject= new JSONObject();
			jsonObject.put(Config.MECHINE_ID, mechineID);
			jsonObject.put("cmd", "ch");
			jsonObject.put("SendMsg", GetBillObject());
			jsonObject.put("MsgId", "");
			JSONObject samtypeJson= new JSONObject();
			samtypeJson.put(Config.CMD, "chFail");
			samtypeJson.put("upSellRecord",json);
			jsonObject.put("samtype", samtypeJson.toString());
			Util.sendMsg(jsonObject.toString());
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	private void sendchSuccess(String json) {
		// �ύ������¼
		
		try {
			JSONObject jsonObject= new JSONObject();
			jsonObject.put(Config.MECHINE_ID, mechineID);
			jsonObject.put("cmd", "ch");
			jsonObject.put("SendMsg", GetBillObject());
			jsonObject.put("MsgId", "");
			JSONObject samtypeJson= new JSONObject();
			samtypeJson.put(Config.CMD, "chSuccess");
			samtypeJson.put("upSellRecord",json);
			jsonObject.put("samtype", samtypeJson.toString());
			Util.sendMsg(jsonObject.toString());
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	private String tenToSixTeen(int num){
		if(num==10){
			return "A";
		}else if (num==11) {
			return "B";
		}
		else if (num==12) {
			return "C";
		}
		else if (num==13) {
			return "D";
		}
		else if (num==14) {
			return "E";
		}
		else if (num==15) {
			return "F";
		}
		else if (num==16) {
			return "G";
		}else{
			
			return String.valueOf(num);
		}
		
	}
	
}
