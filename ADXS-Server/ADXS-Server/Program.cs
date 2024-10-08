using ADXS.Server.Module.InitialView;
using ADXS.Server.NetWork;
using GameNetLib.Config;
using GameNetLib.Core;
using GameNetLib.Event.NetWork;
using GameNetLib.Utils.Logging;
using Newtonsoft.Json;

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
            MessageHandler handler = new MessageHandler();
            Dictionary<string, object> dic = new Dictionary<string, object>()
            {
                { MessageHandler.messageIdKey,new Dog().eventId},
                {MessageHandler.messageBodyKey, new Dog()}
            };
            var b = handler.SerializeMessage(dic);
            var d = handler.DeserializeMessage(b);
            var id = handler.GetValue<MessageType>(d[MessageHandler.messageIdKey]);
            var body = handler.GetValue<Dog>(d[MessageHandler.messageBodyKey]);
        }
    }
}

public class Dog
{
    public int eventId = 1;
    public string clientIp = "127.0.0.1";
}
