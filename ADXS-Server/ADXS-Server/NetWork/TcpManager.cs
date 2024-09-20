using GameNetLib.NetWork;
using GameNetLib.NetWork.Message;
using GameNetLib.Utils.Singleton;

namespace ADXS.Server.NetWork
{
    public class TcpManager : SingletonAsync<TcpManager>
    {
        private TcpServer tcpServer = null;
        private MessageHandler messageHandler = null;

        public void Init()
        {
            tcpServer = new TcpServer();
            messageHandler = new MessageHandler();
            tcpServer.Init(messageHandler);
        }

        public void AddEvent(MessageType messageType, IAnalyzeMessage action)
        {
            messageHandler.AddEvent((ushort)messageType, action);
        }

        public void Remove(MessageType messageType)
        {
            messageHandler.RemoveEvent((ushort)messageType);
        }

        public void Send<T>(string clientIp, MessageType messageType, T model)
        {
            tcpServer.Send(clientIp, (ushort)messageType, model);
        }

        public void SendAll<T>(MessageType messageType, T model)
        {
            tcpServer.SendAll((ushort)messageType, model);
        }

    }
}
