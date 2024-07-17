using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pman
{
    class user
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public byte[] salt { get; set; }
    }
}
