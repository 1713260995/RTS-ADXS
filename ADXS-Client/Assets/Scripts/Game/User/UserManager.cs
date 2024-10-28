using Assets.GameClientLib.Utils.Singleton;
using Assets.Scripts.Modules.User;
using System;

namespace Assets.Scripts.Manager
{
    public class UserManager : Singleton<UserManager>
    {
        public User user { get; set; }
        private Login login = null;


        public UserManager()
        {
            login = new Login();
        }

        public void Login(User user)
        {
            login.Send(user);
        }

        public void Register()
        {

        }



    }
}
