using ADXS.Server.NetWork;
using GameNetLib.Config;
using GameNetLib.Core;
using GameNetLib.NetWork;
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
            ServerInitializer.Init(EnvironmentMode.release);
            TcpManager.Instance.Init();
        }
    }
}
