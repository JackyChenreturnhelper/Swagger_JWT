using System;
using System.Collections.Generic;
using System.Text;

namespace Swagger_JWT.Repository.Model
{
  public  class ApiClaims
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
