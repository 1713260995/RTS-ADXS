using ADXS.Server.Module;
using GameNetLib.Common.Event;
using GameNetLib.Config;
using GameNetLib.NetWork.Base;
using GameNetLib.NetWork.Message;
using GameNetLib.NetWork.Tcp;

namespace ADXS.Server.NetWork
{
    public class TcpManager : NetWorkManager<TcpManager, TcpMessageArgs, TcpMessageType>
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
            Subscribe(TcpMessageType.Login, new Login());

        }



    }
}
