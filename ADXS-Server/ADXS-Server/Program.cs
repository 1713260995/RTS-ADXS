using ADXS.Server.NetWork;
using GameNetLib.Config;
using GameNetLib.Core;
using GameNetLib.Utils.Logging;

namespace ADXS.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Init();
            LogSystem.Log("Service Started...");
            Console.ReadKey();
        }


        private static void Init()
        {
            ServerInitializer.Init(EnvironmentMode.release);
            TcpManager.Instance.Init();
            UdpManager.Instance.Init();
            Test();
        }

        private static void Test()
        {

        }
    }
}


