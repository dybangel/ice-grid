package com.sq.util;
import android.os.Handler;
import android.os.Message;
public class MainPresenterBroast {
	public final static int MSG_SHOW_TIPS = 0x01;
	private IMainView mMainView;
	private MainHandler mMainHandler;
	private final int A = 1000 * 60 * 60;
	private boolean tipsIsShowed = true;
	private Runnable tipsShowRunable = new Runnable() {
		@Override
		public void run() {
			mMainHandler.obtainMessage(MSG_SHOW_TIPS).sendToTarget();
		}
	};
	public MainPresenterBroast(IMainView view) {
		mMainView = view;
		mMainHandler = new MainHandler();
	}
	/**
	 * 
	 * <�޲���ʱ��ʼ��ʱ> <������ϸ����>
	 * 
	 * @see [�ࡢ��#��������#��Ա]
	 */
	public void startTipsTimer() {
		mMainHandler.postDelayed(tipsShowRunable, A);
	}

	/**
	 * 
	 * <������ǰ��ʱ,���ü�ʱ> <������ϸ����>
	 * 
	 * @see [�ࡢ��#��������#��Ա]
	 */
	public void endTipsTimer() {
		mMainHandler.removeCallbacks(tipsShowRunable);
	}

	public void resetTipsTimer() {
		tipsIsShowed = false;
		mMainHandler.removeCallbacks(tipsShowRunable);
		mMainHandler.postDelayed(tipsShowRunable, 5000);
	}

	public class MainHandler extends Handler {
		@Override
		public void handleMessage(Message msg) {
			super.handleMessage(msg);
			switch (msg.what) {
			case MSG_SHOW_TIPS:
				mMainView.showTipsView();
				tipsIsShowed = true;
				// ������ʾ,�������������¼���Enter���ɹر�����
				break;
			}

		}

	}
}
