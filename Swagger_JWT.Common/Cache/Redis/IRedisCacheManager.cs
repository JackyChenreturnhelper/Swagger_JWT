using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Common.Cache.Redis
{
    /// <summary>
    /// Redis緩存接口
    /// </summary>
    public interface IRedisCacheManager
    {

        //獲取 Reids 緩存值
        string GetValue(string key);

        //獲取值，並序列化
        TEntity Get<TEntity>(string key);

        //保存
        void Set(string key, object value, TimeSpan cacheTime);

        //判斷是否存在
        bool Get(string key);

        //移除某一個緩存值
        void Remove(string key);

        //全部清除
        void Clear();
    }
}
