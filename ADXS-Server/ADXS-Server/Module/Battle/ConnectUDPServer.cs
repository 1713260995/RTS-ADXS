using ADXS.Server.NetWork;
using GameNetLib.Common.Event;
using GameNetLib.NetWork.Udp;

namespace ADXS.Server.Module.Battle
{
    public class ConnectUDPServer : IEventHandler<UdpMessageArgs>
    {
        public async Task EventHandler(UdpMessageArgs args)
        {
            int port = args.GetBody<int>();
            UdpManager.Instance.SaveClientPoint(args.remoteIp, port);
            await UdpManager.Instance.Send(UdpMessageType.Connect, args.remoteIp, true);
        }
    }
}
