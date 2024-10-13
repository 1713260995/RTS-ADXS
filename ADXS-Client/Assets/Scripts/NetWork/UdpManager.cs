using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Base;
using Assets.GameClientLib.Scripts.Network.Message;
using Assets.GameClientLib.Scripts.Network.Udp;
using GameNetLib.NetWork;

namespace Assets.Scripts.NetWork
{
    public class UdpManager : NetWorkManager<UdpManager, MessageType>
    {
        public override void Init()
        {
            eventSystem = new EventSystem<MessageArgs>();
            messageHandler = new MessageSerializeByJson();
            netConfig = GlobalConfig.Instance.netConfig;
            server = new UdpServer(netConfig, messageHandler, eventSystem);
            server.Start();
            InitListenEvent();
        }

        protected override void InitListenEvent()
        {

        }
    }
}
