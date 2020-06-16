using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Common.LoginUser
{
    public interface ILoginUser
    {
        /// <summary>
        /// 取得是否已經登入
        /// </summary>
        /// <value><c>true</c> if this instance is login; otherwise, <c>false</c>.</value>
        bool IsLogin { get; }

        /// <summary>
        /// 取得登入員工編號
        /// </summary>
        string EmployeeId { get; set; }

        /// <summary>
        /// 取得登入員工職稱編號
        /// </summary>
        string TitleId { get; }

        /// <summary>
        /// 取得登入員工的部門代號
        /// </summary>
        string DepartmentId { get; }

        /// <summary>
        /// 資料可見等級
        /// </summary>
        int ViewLevel { get; }
        public string Email { get; set; }
    }
}
