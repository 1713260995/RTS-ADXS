using Assets.GameClientLib.Utils.Singleton;
using Assets.Scripts.Modules.User;
using System;

namespace Assets.Scripts.Manager
{
    public class UserManager : Singleton<UserManager>
    {
        public User user = null;
        private Login logon = null;


        public UserManager()
        {
            logon = new Login();
        }

        public void Login(User user)
        {
            logon.Send(user);
        }

        public void Register()
        {

        }



    }
}
