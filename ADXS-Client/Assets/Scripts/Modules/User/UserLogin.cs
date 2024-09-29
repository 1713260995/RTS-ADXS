using Assets.GameClientLib.Network.Message;
using Assets.Scripts.NetWork;
using Assets.Scripts.Scene;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules.User
{

    public class UserLogin : IAnalyzeMessage
    {

        private Action<User> loginCallback { get; set; }

        public UserLogin()
        {
            TcpManager.Instance.AddEvent(MessageType.Login, this);
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

        public void AnalyzeMessage(byte[] body)
        {
            User user = TcpManager.Instance.DeserializeModel<User>(body);
            UserManager.Instance.user = user;
            loginCallback?.Invoke(user);

            PlayerPrefs.SetString(StartupSceneCtrl.userKey, JsonConvert.SerializeObject(user));
        }
    }
}
