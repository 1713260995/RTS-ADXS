using ADXS.Server.Entity.User;
using GameNetLib.Database;
using GameNetLib.Utils.Unique;
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

        public InitialView()
        {
            userTable = new MysqlTableOperate<User>();
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
                    { nameof(user.createDate),"9.23"},
                },
                new Dictionary<string, object>
                {
                      { nameof(user.pwd),"123456"},
                }
            );
        }

    }
}
