using Castle.DynamicProxy;
using Swagger_JWT.Common.Cache.MemoryCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swagger_JWT.Common.AOP
{
    /// <summary>
    /// 面向切面的緩存使用
    /// </summary>
    public class CacheAOP : CacheAOPbase
    {
        //通過注入的方式，把緩存操作接口通過構造函數注入
        private readonly ICaching _cache;
        public CacheAOP(ICaching cache)
        {
            _cache = cache;
        }

        //Intercept方法是攔截的關鍵所在，也是IInterceptor接口中的唯一定義
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //對當前方法的特性驗證
            //如果需要驗證
            if (method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) is CachingAttribute qCachingAttribute)
            {
                //獲取自定義緩存鍵
                var cacheKey = CustomCacheKey(invocation);
                //根據key獲取相應的緩存值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //將當前獲取到的緩存值，賦值給當前執行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去執行當前的方法
                invocation.Proceed();
                //存入緩存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue);
                }
            }
            else
            {
                invocation.Proceed();//直接執行被攔截方法
            }
        }
    }
}
