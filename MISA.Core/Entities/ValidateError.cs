using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Entities
{
    public class ValidateError
    {
        public string DevMsg { get; set; }
        public string UserMsg { get; set; }
        public object Data { get; set; }
    }
}
