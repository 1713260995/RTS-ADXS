using ADXS.Server.Module.InitialView;
using GameNetLib.Utils.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace ADXS.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Launcher.Init();
            Test();
            LogSystem.Log("Service Started...");
            Console.ReadKey();
        }


        static void Test()
        {
            InitialView view = new InitialView();
            view.Test();
        }
    }
}
