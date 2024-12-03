using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Base;
using Assets.GameClientLib.Scripts.Network.Message;
using Assets.GameClientLib.Scripts.Network.Udp;

namespace Assets.Scripts.NetWork
{
    public class UdpManager : NetworkManager<UdpManager, UdpMessageArgs, UdpMessageType>
    {
        private UdpServer udpServer;
        public int localPort => udpServer.port;

        public override void Init()
        {
            eventSystem = new EventSystem<UdpMessageArgs>();
            messageHandler = new MessageSerializeByJson<UdpMessageArgs>();
            netConfig = GlobalConfig.netConfig;
            udpServer = new UdpServer(netConfig, messageHandler, eventSystem);
            server = udpServer;
            server.Start();
            InitListenEvent();
        }

    }
}
