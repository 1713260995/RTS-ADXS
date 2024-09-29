using Assets.GameClientLib.Scripts.Config.SO;
using Assets.GameClientLib.Scripts.Game;
using Assets.GameClientLib.Utils.Json;
using Assets.Scripts.Game;
using Assets.Scripts.Modules.User;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class StartupSceneCtrl : MonoBehaviour
    {
        public static InitConfigSO initSO;
        public const string userKey = "user";

        public void Start()
        {
            GameManager.Instance.onInitCompleted += QuickLogin;
        }


        public void QuickLogin()
        {
            var json = PlayerPrefs.GetString(userKey);
            User user = JsonConvert.DeserializeObject<User>(json);
            user = new User
            {
                account = "123",
                pwd = "345",
            };
            if (user == null)
            {
                GameScene.LoadAsync(SceneName.Lobby, ToLobby);
            }
            else
            {
                UserManager.Instance.Login(user.account, user.pwd, (user) =>
                {
                    GameScene.LoadAsync(SceneName.Lobby, ToLobby);
                });
            }
        }


        public async UniTask ToLobby(AsyncOperation a)
        {
            await UniTask.Delay(3000);
            a.allowSceneActivation = true;
        }

        #region Initialize

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void GameInitialize()
        {
            GlobalJsonSetting();
            initSO = Resources.Load<InitConfigSO>("InitConfig");
            if (initSO.autoLoadGameManager)
            {
                _ = GameManager.Instance;
            }
        }

        private static void GlobalJsonSetting()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ContractResolver = new PrivateSetterContractResolver(),//允许序列化 private set 属性
            };

            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                setting.NullValueHandling = NullValueHandling.Ignore;  //空值处理
                return setting;
            });
        }

        #endregion
    }
}
