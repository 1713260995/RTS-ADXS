using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Event.NetWork;
using Assets.Scripts.NetWork;
using Assets.Scripts.Scene;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules.User
{

    public class UserLogin : IEventHandler<TcpEventArgs>
    {

        private Action<User> loginCallback { get; set; }

        public UserLogin()
        {
            TcpManager.Instance.Subscribe(MessageType.Login, this);
        }

        public void Send(string _account, string _pwd, Action<User> callback)
        {
            User user = new User()
            {
                account = _account,
                pwd = _pwd,
            };
            TcpManager.Instance.Send(MessageType.Login, user);
            loginCallback = callback;
        }

        public void EventHandler(TcpEventArgs args)
        {
            User user = TcpManager.Instance.DeserializeModel<User>(args.body);
            UserManager.Instance.user = user;
            loginCallback?.Invoke(user);
            PlayerPrefs.SetString(StartupSceneCtrl.userKey, JsonConvert.SerializeObject(user));
        }
    }
}
