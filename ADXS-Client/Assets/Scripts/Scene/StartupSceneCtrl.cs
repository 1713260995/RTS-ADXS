using Assets.GameClientLib.Scripts.Game;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Game;
using Assets.Scripts.Manager;
using Assets.Scripts.Modules.User;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scene
{
    public class StartupSceneCtrl : MonoBehaviour
    {
        public const string userKey = "user";

        [SerializeField]
        private GameManager gameManagerPrefab;

        private void Awake()
        {
            GameManager manager = Instantiate(gameManagerPrefab);
            manager.AddInitCompletedEvent(QuickBattle);
            //manager.AddInitCompletedEvent(QuickLogin);
        }

        private void QuickBattle()
        {
            GameSceneManager.Instance.Load(SceneName.Battle);
        }

        private void QuickLogin()
        {
            var json = PlayerPrefs.GetString(userKey);
            User user = JsonConvert.DeserializeObject<User>(json) ?? new();
            UserManager.Instance.Login(user);
        }

    }
}
