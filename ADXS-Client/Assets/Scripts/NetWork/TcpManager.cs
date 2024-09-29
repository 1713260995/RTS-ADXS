using Assets.GameClientLib.Network.Message;
using Assets.GameClientLib.Utils.Singleton;
using GameNetLib.NetWork;
using UnityEngine;

namespace Assets.Scripts.NetWork
{
    public class TcpManager : Singleton<TcpManager>
    {
        private TcpClient tcpClient;
        private IMessageHandler messageHandler;

        public void Init()
        {
            tcpClient = new TcpClient();
            messageHandler = new MessageHandler();
            tcpClient.Init(messageHandler);
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

        /// <summary>
        /// 添加事件处理服务器发送的消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageType"></param>
        /// <param name="analyzeEvent"></param>
        /// <param name="isOnlyEvent"></param>
        public void AddEvent<T>(MessageType messageType, T analyzeEvent, bool isOnlyEvent = true) where T : IAnalyzeMessage
        {
            if (isOnlyEvent && ContainEvent<T>(messageType))
            {
                Debug.LogError($"{messageType},{nameof(analyzeEvent)}事件无法被重复添加");
                return;
            }
            messageHandler.AddAnalyzeEvent((ushort)messageType, analyzeEvent);
        }

        public bool ContainEvent<T>(MessageType messageType) where T : IAnalyzeMessage
        {
            return messageHandler.ContainAnalyzeEvent<T>((ushort)messageType);
        }

        public void RemoveEvent(MessageType messageType, IAnalyzeMessage action)
        {
            messageHandler.RemoveAnalyzeEvent((ushort)messageType, action);
        }

        public void Send<T>(MessageType messageType, T model)
        {
            tcpClient.Send((ushort)messageType, model);
        }

        public override void Destory()
        {
            base.Destory();
            tcpClient.Close();
        }
    }
}
