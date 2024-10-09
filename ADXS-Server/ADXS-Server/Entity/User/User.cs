using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADXS.Server.Entity.User
{
    public class User
    {
        public int id { get; set; }
        public string account { get; set; }
        public string pwd { get; set; }
        public string ip { get; set; }
        public DateTime createTime { get; set; }
    }
}
