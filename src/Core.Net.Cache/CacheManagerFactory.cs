using Core.Net.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Net.Cache
{
    public static partial class CacheManagerFactory
    {
        private static ICacheManager _instance = null;
        /// <summary>
        /// 静态实例，外部可直接调用
        /// </summary>
        public static ICacheManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    ////开启了Redis服务和Redis缓存
                    //if (AppSettingsConstVars.RedisConfigEnabled&& AppSettingsConstVars.RedisConfigUseRedisCache)
                    //{
                    //    _instance = new RedisCacheManager();
                    //}
                    //else
                    //{
                        _instance = new MemoryCacheManager();

                    //}
                }

                return _instance;
            }
        }
    }
}
