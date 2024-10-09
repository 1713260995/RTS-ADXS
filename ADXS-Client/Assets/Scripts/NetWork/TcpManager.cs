using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Message;
using Assets.GameClientLib.Utils.Singleton;
using GameNetLib.NetWork;
using UnityEngine.EventSystems;

namespace Assets.Scripts.NetWork
{
    public class TcpManager : Singleton<TcpManager>
    {
        private TcpClient tcpClient;
        private NetworkMsgHandler messageHandler;
        private EventSystem<TcpEventArgs> eventSystem;


        public void Init()
        {
            tcpClient = new TcpClient();
            messageHandler = new NetworkMsgHandler();
            eventSystem = new EventSystem<TcpEventArgs>();
            tcpClient.Init(messageHandler, eventSystem, GlobalConfig.Instance.netConfig);
            tcpClient.Start();
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

        public void Send<T>(MessageType messageType, T model)
        {
            tcpClient.Send((ushort)messageType, model);
        }

        public override void Destory()
        {
            base.Destory();
            tcpClient?.Close();
        }
    }
}
