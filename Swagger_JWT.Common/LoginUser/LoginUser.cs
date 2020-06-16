using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Common.LoginUser
{
    public class LoginUser : ILoginUser
    {/// <summary>
     /// 登入員工編號 </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// 登入員工的部門代號
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 是否已經登入
        /// </summary>
        /// <value><c>true</c> if this instance has logged in; otherwise, <c>false</c>.</value>
        public bool IsLogin { get; set; }

        /// <summary>
        /// 登入員工職稱編號
        /// </summary>
        public string TitleId { get; set; }

        /// <summary>
        /// 登入員工姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 模擬員工姓名
        /// </summary>
        public string SimulationName { get; set; }

        /// <summary>
        /// 模擬員工編號
        /// </summary>
        public string SimulationId { get; set; }

        /// <summary>
        /// 登入狀態 (正常、模擬)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 資料可見等級
        /// </summary>
        public int ViewLevel { get; set; }

        public string Email { get; set; }
    }
}
