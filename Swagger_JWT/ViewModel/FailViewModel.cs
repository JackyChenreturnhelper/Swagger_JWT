using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swagger_JWT.ViewModel
{
    public class FailViewModel<T>
    {
        public string ApiVersion { get; set; }
        public IEnumerable<T> Error { get; set; }
        public Guid Id { get; set; }
        public string Method { get; set; }
        public string Status { get; set; }
    }
}
