using ADXS.Server.Entity.User;
using ADXS.Server.NetWork;
using GameNetLib.Utils.Unique;
using GameNetLib.Database;
using System.Security.Principal;
using GameNetLib.NetWork.Message;
using GameNetLib.Common.Event;

namespace ADXS.Server.Module.InitialView
{
    public class Login : IEventHandler<MessageArgs>
    {
        MysqlTableOperate<User> db = new MysqlTableOperate<User>();

        public void EventHandler(MessageArgs args)
        {
            User user = args.GetBody<User>();
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
            _ = TcpManager.Instance.Send(args.clientIp, MessageType.Login, realUser);
        }
    }
}
