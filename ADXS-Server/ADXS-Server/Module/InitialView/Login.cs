using ADXS.Server.Entity.User;
using ADXS.Server.NetWork;
using GameNetLib.Event.NetWork;
using GameNetLib.Event;
using GameNetLib.Utils.Unique;
using GameNetLib.Database;
using System.Security.Principal;

namespace ADXS.Server.Module.InitialView
{
    public class Login : IEventHandler<TcpEventArgs>
    {
        MysqlTableOperate<User> db = new MysqlTableOperate<User>();

        public void EventHandler(TcpEventArgs args)
        {
            User user = TcpManager.Instance.DeserializeModel<User>(args.body);
            Dictionary<string, object> query = new Dictionary<string, object>
            {
                { "account", user.account },
                { "pwd", user.pwd }
            };

            User realUser = db.FindOne(query);
            if (realUser != null)
            {
                realUser.ip = args.clientIp;
            }
            TcpManager.Instance.Send(args.clientIp, MessageType.Login, realUser);
        }
    }
}
