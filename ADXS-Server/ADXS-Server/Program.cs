using ADXS.Server.Module.InitialView;
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
            Test();
            LogSystem.Log("Service Started...");
            Console.ReadKey();
        }


        private static void Init()
        {
            ServerInitializer.Init(EnvironmentMode.release);
            TcpManager.Instance.Init();
        }

        private static void Test()
        {
            InitialView view = new InitialView();
            //  view.Test();
        }
    }
}
