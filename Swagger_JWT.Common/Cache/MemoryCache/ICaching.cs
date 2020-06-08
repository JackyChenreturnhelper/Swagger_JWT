using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Common.Cache.MemoryCache
{
    /// <summary>
    /// 簡單的緩存接口，只有查詢和添加，以後會進行擴展
    /// </summary>
    public interface ICaching
    {
        object Get(string cacheKey);

        void Set(string cacheKey, object cacheValue);
    }
}
