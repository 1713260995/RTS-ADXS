using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Base;
using Assets.GameClientLib.Scripts.Network.Message;
using Assets.GameClientLib.Scripts.Network.Tcp;
using Assets.GameClientLib.Scripts.Network.Udp;
using Assets.GameClientLib.Utils.Singleton;
using GameNetLib.NetWork;
using UnityEngine.EventSystems;

namespace Assets.Scripts.NetWork
{
    public class TcpManager : NetworkManager<TcpManager, TcpMessageArgs, TcpMessageType>
    {
        public override void Init()
        {
            eventSystem = new EventSystem<TcpMessageArgs>();
            messageHandler = new MessageSerializeByJson<TcpMessageArgs>();
            netConfig = GlobalConfig.Instance.netConfig;
            server = new TcpServer(netConfig, messageHandler, eventSystem);
            server.Start();
            InitListenEvent();
        }

        protected override void InitListenEvent()
        {

        }


    }
}
