using Castle.DynamicProxy;
using Swagger_JWT.Common.Cache.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swagger_JWT.Common.AOP
{
    /// <summary>
    /// 面向切面的緩存使用
    /// </summary>
    public class RedisCacheAOP : CacheAOPbase
    {
        //通過注入的方式，把緩存操作接口通過構造函數注入
        private readonly IRedisCacheManager _cache;
        public RedisCacheAOP(IRedisCacheManager cache)
        {
            _cache = cache;
        }

        //Intercept方法是攔截的關鍵所在，也是IInterceptor接口中的唯一定義
        //代碼已經合並 ，學習pr流程
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
            {
                invocation.Proceed();
                return;
            }
            //對當前方法的特性驗證
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute)) as CachingAttribute;

            if (qCachingAttribute != null)
            {
                //獲取自定義緩存鍵
                var cacheKey = CustomCacheKey(invocation);
                //注意是 string 類型，方法GetValue
                var cacheValue = _cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                    //將當前獲取到的緩存值，賦值給當前執行方法
                    Type returnType;
                    if (typeof(Task).IsAssignableFrom(method.ReturnType))
                    {
                        returnType = method.ReturnType.GenericTypeArguments.FirstOrDefault();
                    }
                    else
                    {
                        returnType = method.ReturnType;
                    }

                    dynamic _result = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, returnType);
                    invocation.ReturnValue = (typeof(Task).IsAssignableFrom(method.ReturnType)) ? Task.FromResult(_result) : _result;
                    return;
                }
                //去執行當前的方法
                invocation.Proceed();

                //存入緩存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;

                    //Type type = invocation.ReturnValue?.GetType();
                    var type = invocation.Method.ReturnType;
                    if (typeof(Task).IsAssignableFrom(type))
                    {
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) response = string.Empty;

                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接執行被攔截方法
            }
        }
    }
}
