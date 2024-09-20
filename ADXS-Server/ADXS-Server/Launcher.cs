using ADXS.Server.NetWork;
using GameNetLib.Config;
using GameNetLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADXS.Server
{
    public class Launcher
    {
        public static void Init()
        {
            ServerInitializer.Init();
            NetConfig.Instance.Init("127.0.0.1", 7000);
            TcpManager.Instance.Init();
        }
    }
}
