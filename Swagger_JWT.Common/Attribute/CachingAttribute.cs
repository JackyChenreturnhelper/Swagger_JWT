using System;


namespace Swagger_JWT.Common
{
    /// <summary>
    /// 這個Attribute就是使用時候的驗證，把它添加到要緩存數據的方法中，即可完成緩存的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        /// <summary>
        /// 緩存絕對過期時間（分鐘）
        /// </summary>
        public int AbsoluteExpiration { get; set; } = 30;
    }
}