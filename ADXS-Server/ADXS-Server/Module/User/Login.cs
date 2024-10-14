using ADXS.Server.Entity.User;
using ADXS.Server.NetWork;
using GameNetLib.Database;
using GameNetLib.Common.Event;
using GameNetLib.NetWork.Tcp;

namespace ADXS.Server.Module
{
    public class Login : IEventHandler<TcpMessageArgs>
    {
        MysqlTable<User> table = new MysqlTable<User>();

        public async Task EventHandler(TcpMessageArgs args)
        {
            User user = args.GetBody<User>();
            Dictionary<string, object> query = new Dictionary<string, object>
            {
                { "account", user.account },
                { "pwd", user.pwd }
            };

            User realUser = table.FindOne(query);
            if (realUser != null)
            {
                realUser.ip = args.remoteIp;
            }

            await TcpManager.Instance.Send(TcpMessageType.Login, args.remoteIp, realUser);
        }
    }
}
