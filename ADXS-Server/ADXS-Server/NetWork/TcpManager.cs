using ADXS.Server.Module.InitialView;
using GameNetLib.Config;
using GameNetLib.Event;
using GameNetLib.Event.NetWork;
using GameNetLib.NetWork.Message;
using GameNetLib.NetWork.Tcp;
using GameNetLib.Utils.Singleton;
using System;

namespace ADXS.Server.NetWork
{
    public class TcpManager : SingletonAsync<TcpManager>
    {
        private TcpServer tcpServer;
        private INetworkMsgHandler messageHandler;
        private EventSystem<TcpEventArgs> eventSystem;

        public void Init()
        {
            tcpServer = new TcpServer();
            messageHandler = new NetworkMsgHandler();
            eventSystem = new EventSystem<TcpEventArgs>();
            tcpServer.Init(messageHandler, eventSystem, GlobalConfig.Instance.netConfig);
            AddListen();
        }

        public void AddListen()
        {
            Subscribe(MessageType.Login, new Login());
        }



        public T DeserializeModel<T>(byte[] body)
        {
            return messageHandler.DeserializeModel<T>(body);
        }

        public byte[] SerializeMessage<T>(MessageType messageType, T model)
        {
            return messageHandler.SerializeMessage((ushort)messageType, model);
        }

        public void Subscribe(MessageType messageType, IEventHandler<TcpEventArgs> handler)
        {
            eventSystem.Subscribe((int)messageType, handler);
        }

        public void Unsubscribe(MessageType messageType, IEventHandler<TcpEventArgs> handler)
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
