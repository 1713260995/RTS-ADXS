using ADXS.Server.Module;
using ADXS.Server.Module.Battle;
using GameNetLib.Common.Event;
using GameNetLib.Config;
using GameNetLib.NetWork.Base;
using GameNetLib.NetWork.Message;
using GameNetLib.NetWork.Udp;
using GameNetLib.Utils.Logging;

namespace ADXS.Server.NetWork
{
    public class UdpManager : NetWorkManager<UdpManager, UdpMessageArgs, UdpMessageType>
    {
        private UdpServer udpServer { get; set; }
        public override void Init()
        {
            eventSystem = new EventSystem<UdpMessageArgs>();
            netConfig = GlobalConfig.Instance.netConfig;
            messageHandler = new MessageSerializeByJson<UdpMessageArgs>();
            udpServer = new UdpServer(netConfig, messageHandler, eventSystem);
            server = udpServer;
            server.Start();
            InitListenEvent();
            InitPublishBeforeEvent();
        }

        protected override void InitListenEvent()
        {
            Subscribe(UdpMessageType.Connect, new ConnectUDPServer());
        }

        protected void InitPublishBeforeEvent()
        {
            AddPublishBeforeEvent(ConnectionVerification);
        }

        /// <summary>
        /// 通信校验
        /// </summary>
        public void ConnectionVerification(int eventId, UdpMessageArgs args)
        {
            UdpMessageType type = (UdpMessageType)eventId;
            if (type != UdpMessageType.Connect && !udpServer.IsExistClientPoint(args.remoteIp))
            {
                throw new Exception("UDP-无法与没有创建连接的客户端进行udp通信");
            }
        }

        public void SaveClientPoint(string remoteIp, int port)
        {
            udpServer.SaveClientPoint(remoteIp, port);
        }
    }
}
