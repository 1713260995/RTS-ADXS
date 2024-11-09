using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Config.SO;
using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.GameClientLib.Utils.Json;
using Assets.Scripts.NetWork;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Assets.GameClientLib.Scripts.Game
{
    public class GameManager : SingletonMono<GameManager>
    {
        private event Action onInitCompleted;
        private event Action onGameQuitEvent;


        [SerializeField]
        private InitConfigSO initSO;

        protected override void Awake()
        {
            base.Awake();
            Init().Forget();
        }

        public void OnDestroy()
        {
            onGameQuitEvent?.Invoke();
        }

        #region Init

        /// <summary>
        /// 初始化全局配置
        /// 方法调用顺序不能随意调换
        /// </summary>
        /// <returns></returns>
        public async UniTask Init()
        {
            GlobalJsonSetting();
            await GlobalConfig.Instance.Init(initSO);
            if (initSO.networkConnectMode != NetworkConnectMode.SinglePlayer)
            {
                TcpManager.Instance.Init();
                UdpManager.Instance.Init();
            }
            onInitCompleted?.Invoke();
        }

        public void AddInitCompletedEvent(Action action)
        {
            onInitCompleted += action;
        }

        public void AddGameQuitEvent(Action action)
        {
            onGameQuitEvent += action;
        }

        private void GlobalJsonSetting()
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
