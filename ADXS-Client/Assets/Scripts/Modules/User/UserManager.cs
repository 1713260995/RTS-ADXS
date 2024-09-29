using Assets.GameClientLib.Utils.Singleton;
using System;

namespace Assets.Scripts.Modules.User
{
    public class UserManager : Singleton<UserManager>
    {
        public User user = null;
        private UserLogin logon = null;


        public UserManager()
        {
            logon = new UserLogin();
        }

        public void Login(string id, string pwd, Action<User> callback = null)
        {
            logon.Send(id, pwd, callback);
        }

        public void Register()
        {

        }



    }
}
