using GameNetLib.Common.Event;
using GameNetLib.Config;
using GameNetLib.NetWork.Base;
using GameNetLib.NetWork.Message;
using GameNetLib.NetWork.Udp;

namespace ADXS.Server.NetWork
{
    public class UdpManager : NetWorkManager<UdpManager, MessageType>
    {
        public override void Init()
        {
            eventSystem = new EventSystem<MessageArgs>();
            netConfig = GlobalConfig.Instance.netConfig;
            messageHandler = new MessageSerializeByJson();
            server = new UdpServer(netConfig, messageHandler, eventSystem);
            server.Start();
            InitListenEvent();
        }

        protected override void InitListenEvent()
        {

        }
    }
}
