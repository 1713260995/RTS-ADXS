using Assets.GameClientLib.Scripts.Event;
using Assets.GameClientLib.Scripts.Network.Message;
using Assets.GameClientLib.Scripts.Network.Tcp;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Game;
using Assets.Scripts.Manager;
using Assets.Scripts.NetWork;
using Assets.Scripts.Scene;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules.User
{

    public class Login : IEventHandler<TcpMessageArgs>
    {

        public Login()
        {
            TcpManager.Instance.Subscribe(TcpMessageType.Login, this);
        }

        public void Send(User user)
        {
            TcpManager.Instance.Send(TcpMessageType.Login, user);
        }

        public void EventHandler(TcpMessageArgs args)
        {
            User user = args.GetBody<User>();
            if (user == null)
            {
                Debug.Log("登录失败");
            }
            else
            {
                var json = JsonConvert.SerializeObject(user);
                Debug.Log("登录成功,用户信息：" + json);
                UserManager.Instance.user = user;
                PlayerPrefs.SetString(StartupSceneCtrl.userKey, JsonConvert.SerializeObject(user));
                GameSceneManager.Instance.LoadAsync(SceneName.Lobby, ToLobby);
            }
        }


        public async UniTask ToLobby(AsyncOperation a)
        {
            await UniTask.Delay(3000);
            a.allowSceneActivation = true;
        }
    }
}
