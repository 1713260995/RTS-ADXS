using GameNetLib.Event;
using GameNetLib.Event.NetWork;
using GameNetLib.NetWork.Message;
using GameNetLib.NetWork.Tcp;
using GameNetLib.Utils.Singleton;

namespace ADXS.Server.NetWork
{
    public class TcpManager : SingletonAsync<TcpManager>
    {
        private TcpServer tcpServer;
        private INetworkMsgHandler messageHandler;
        private IEventSystem eventSystem;

        public void Init()
        {
            tcpServer = new TcpServer();
            messageHandler = new NetworkMsgHandler();
            eventSystem = new TcpEventSystem();
            tcpServer.Init(messageHandler, eventSystem);
        }

        public T DeserializeModel<T>(byte[] body)
        {
            return messageHandler.DeserializeModel<T>(body);
        }

        public byte[] SerializeMessage<T>(MessageType messageType, T model)
        {
            return messageHandler.SerializeMessage<T>((ushort)messageType, model);
        }

        public void AddEvent(MessageType messageType, IEventHandler handler)
        {
            eventSystem.Subscribe((int)messageType, handler);
        }

        public void RemoveEvent(MessageType messageType, IEventHandler handler)
        {
            eventSystem.Unsubscribe((int)messageType, handler);
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
