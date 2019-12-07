using System;
using System.ServiceProcess;
using System.Text;
using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Consumer.cls
{
    public class RedisHelper
    {
        #region Redis连接信息
        //PooledRedisClientManager 是ServiceStack.Redis的连接池管理类
        //public static string RedisPath = "127.0.0.1:6379";
        public static string RedisPath = "r-2zekq2u2wylrv00r7j@r-2zekq2u2wylrv00r7jpd.redis.rds.aliyuncs.com:6379";
        //创建一个PooledRedisClientManager（连接池管理类）的对象
        public static PooledRedisClientManager prcm = CreateManager(new string[] { RedisPath }, new string[] { RedisPath });
        public static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxReadPoolSize = 5,
                MaxWritePoolSize = 5,
                AutoStart = true,
            });
        }
        #endregion

        #region Redis存取对象
        //存对象
        public static bool SetRedisModel<T>(string key, T t, TimeSpan ts)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Set<T>(key, t, ts);
            }
        }
        //存对象
        public static bool SetRedisModel<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Set<T>(key, t);
            }
        }
        //取对象
        public static T GetRedisModel<T>(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Get<T>(key);
            }
        }
        //检验redis中是否含有某个对象
        public static bool CheckReidsModel(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.ContainsKey(key);
            }
        }
        //根据key删除redis数据
        public static bool Remove(string key)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }
        #endregion

        #region redis Set操作
        //判断当前key内，是否有相同的value
        public static bool Set_Contains<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Contains(t);
            }
        }
        //存入redis（先判断在当前key下是否有这个对象）
        public static void Set_Add<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                //redis.set
                var redisTypedClient = redis.As<T>();
                if (!Set_Contains(key, t))
                {
                    redisTypedClient.Sets[key].Add(t);
                }
            }
        }
        //移除redis
        public static bool Set_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Remove(t);
            }
        }
        #endregion

        #region redis Hash操作

        #endregion

        #region 工具
        //序列化
        public static byte[] Serialize(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }
        //反序列化
        public static T DeSerialize<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                return default(T);
            }
            string json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(json);

        }
        #endregion

        #region 服务
        //开启服务
        public static void StartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service != null && service.Status != ServiceControllerStatus.Running)
            {
                service.Start();

            }
        }
        //停止服务
        public static void StopService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service != null && service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
            }
        }
        //重启服务
        public static void ResetService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service != null)
            {
                service.Stop();
                service.Start();
            }
        }
        #endregion
    }
}