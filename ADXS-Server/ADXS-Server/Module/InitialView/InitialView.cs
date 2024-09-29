using ADXS.Server.Entity.User;
using ADXS.Server.NetWork;
using GameNetLib.Database;
using GameNetLib.Event.NetWork;
using GameNetLib.NetWork.Message;
using GameNetLib.Utils.Unique;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADXS.Server.Module.InitialView
{
    public class InitialView
    {
        private User user;
        private MysqlTableOperate<User> userTable;

        private Login login;

        public InitialView()
        {
            userTable = new MysqlTableOperate<User>();
            login = new Login(this);
        }

        private struct Login : ITcpEventHandler
        {
            private InitialView view;
            public Login(InitialView view)
            {
                this.view = view;
                TcpManager.Instance.AddEvent(MessageType.Login, this);
            }

            public void AnalyzeMessage(string clientIp, byte[] body)
            {
                User user = TcpManager.Instance.DeserializeModel<User>(body);
                user.createTiem = DateTime.Now;
                user.id = UniqueData.GenerateUniqueInt();
                view.user = user;
                TcpManager.Instance.Send(clientIp, MessageType.Login, user);
            }

            public void EventHandler(GameNetLib.Event.EventArgs args)
            {
                TcpEventArgs model = (TcpEventArgs)args;
                User user = TcpManager.Instance.DeserializeModel<User>(model.body);
                user.createTiem = DateTime.Now;
                user.id = UniqueData.GenerateUniqueInt();
                view.user = user;
                TcpManager.Instance.Send(model.clientIp, MessageType.Login, user);
            }
        }



        public void Test()
        {
            //var newUser = new User()
            //{
            //    id = UniqueData.GenerateUniqueInt(),
            //    account = "yutyuy",
            //    pwd = "fsdfds"
            //};
            //userTable.Add(newUser);

            //user = new User() { account = "sadfsd", pwd = "324" };
            //var s = userTable.FindCount();
            //userTable.FindOne(new Dictionary<string, object>
            //{
            //    { nameof(user.account),user.account},
            //     { nameof(user.pwd),user.pwd}
            //});

            //List<User> list = userTable.FindAll();
            //user = list.Last();
            ////user.account = "大三大四";
            ////user.pwd = "456das";

            ////userTable.UpdateById(user, user.id);

            //userTable.Delete(user, dic);

            userTable.UpdateByCriteria(
                new Dictionary<string, object>
                {
                    { nameof(user.account),"sadfsd"},
                },
                new Dictionary<string, object>
                {
                      { nameof(user.pwd),"123456"},
                }
            );
        }

    }
}
