using System;
using System.Web;
using System.Xml;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Net.WebSockets;
using System.Threading;
using System.Collections.Generic;
using autosell_center.cls;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace autosell_center.api
{

    public class BlockService
    {

        public static Dictionary<string, Dictionary<string, EventWaitHandle>> eventWait = new Dictionary<string, Dictionary<string, EventWaitHandle>>();
        public static Dictionary<string, Dictionary<string, string>> msgList = new Dictionary<string, Dictionary<string, string>>();

        private BlockModel blockModel;
        object obj = new object() { };

        public BlockService(BlockModel blockModel)
        {
            this.blockModel = blockModel;
        }
        //发指令到指定设备
        public static ArraySegment<byte> sendmsg(string msg)
        {
            ArraySegment<byte> buffer1 = new ArraySegment<byte>(new byte[2048]);
            buffer1 = new ArraySegment<byte>(Encoding.UTF8.GetBytes(msg));
            return buffer1;
        }
        public BlockModel getMsg()
        {
            if (webSocket.CONNECT_POOL.ContainsKey(blockModel.ID))//判断客户端是否在线
            {
                WebSocket destSocket = webSocket.CONNECT_POOL[blockModel.ID];//目的客户端   


                if (destSocket != null && destSocket.State == WebSocketState.Open)
                {
                    //创建此调用的等待
                    EventWaitHandle _waitHandle = new AutoResetEvent(false);


                   
                   

                    if (eventWait.ContainsKey(blockModel.ID) )
                    {
                       
                       
                        eventWait[blockModel.ID].Add(blockModel.MsgId.ToString(), _waitHandle);
                    }
                    else
                    {
                       
                        Dictionary<string, EventWaitHandle> dictionary = new Dictionary<string, EventWaitHandle>();
                        dictionary.Add(blockModel.MsgId.ToString(), _waitHandle);
                        eventWait.Add(blockModel.ID, dictionary);
                       

                    }
                    
                    destSocket.SendAsync(sendmsg(JsonConvert.SerializeObject(blockModel)), WebSocketMessageType.Text, true, CancellationToken.None);
                   
                    Task.Run(() =>
                    {
                        Thread.Sleep(10000);
                       
                        if (eventWait.ContainsKey(blockModel.ID))
                        {
                            
                            Dictionary<string, EventWaitHandle> callMap = eventWait[blockModel.ID];
                            if (callMap.ContainsKey(blockModel.MsgId.ToString()))
                            {
                               
                                EventWaitHandle callBack = callMap[blockModel.MsgId.ToString()];
                                
                                blockModel.Status = 1;
                                callMap.Remove(blockModel.MsgId.ToString());
                                //此处是放开 _waitHandle.WaitOne();等待，继续向下执行，所以此处没有返回值，返回值在最后
                                callBack.Set();
                                
                               
                            }
                        }
                    });
                    _waitHandle.WaitOne();
                    
                    if (msgList.ContainsKey(blockModel.ID ))
                    {
                        if (msgList[blockModel.ID].ContainsKey(blockModel.MsgId.ToString()))
                        {
                            blockModel.Status = 0;
                            blockModel.samtype = msgList[blockModel.ID][blockModel.MsgId.ToString()];
                            msgList[blockModel.ID].Remove(blockModel.MsgId.ToString());
                            //此处是怕因为等待删除和消息新增之间出现问题导致的一些消息没有删除而一直存在消息记录里导致出现垃圾信息影响效率
                            if (msgList[blockModel.ID].Count>=100) {
                                Util.log(msgList[blockModel.ID].ToString(), blockModel.ID + "blockModelMsg.txt");
                                msgList.Remove(blockModel.ID);
                            }
                        }
                        
                    }

                   

                }
                else//连接关闭
                {
                    MechineUtil.updateMechineStatus(blockModel.ID, "1");
                    webSocket.CONNECT_POOL.Remove(blockModel.ID);
                    blockModel.Status = 1;
                }
            }
            else
            {

            }

            return blockModel;
        }
    }

}