using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Service.Dto
{
  public  class ApiClaimsDto
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ApiResourceId { get; set; }
    }
}
