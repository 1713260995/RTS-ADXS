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
            PlayerPrefs.DeleteAll();
            var json = PlayerPrefs.GetString(userKey);
            User user = JsonConvert.DeserializeObject<User>(json) ?? new();
            user.account = "sadfsd";
            user.pwd = "123456";
            UserManager.Instance.Login(user);
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
                return setting;
            });
        }

        #endregion
    }
}
