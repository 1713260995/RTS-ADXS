using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Udp;
using Assets.Scripts.NetWork;
using UnityEngine;

namespace Assets.Scripts.Modules.Battle
{
    public class ConnectUDPServer : IEventHandler<UdpMessageArgs>
    {
        public ConnectUDPServer()
        {
            UdpManager.Instance.Subscribe(UdpMessageType.Connect, this);
        }

        public void Send()
        {
            UdpManager.Instance.Send(UdpMessageType.Connect, UdpManager.Instance.localPort);
        }

        public void EventHandler(UdpMessageArgs args)
        {
            bool result = args.GetBody<bool>();
            if (result)
            {
                Debug.Log("连接udp服务器成功");
            }
            else
            {
                Debug.Log("连接udp服务器失败");
            }
        }
    }
}
