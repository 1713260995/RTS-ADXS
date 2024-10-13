using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Config.SO;
using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.NetWork;
using System;
using Cysharp.Threading.Tasks;
using Assets.GameClientLib.Utils.Json;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.GameClientLib.Scripts.Game
{
    public class GameManager : SingletonMono<GameManager>
    {
        private event Action OnInitCompleted = null;

        [SerializeField]
        private InitConfigSO initSO;

        protected override void Awake()
        {
            base.Awake();
            Init().Forget();
        }

        public void OnDestroy()
        {
            ResourceSystem.Destroy();
            ExceptionSystem.OnDisable();
            TcpManager.Instance.Destory();
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
            ExceptionSystem.OnEnable();
            ResourceSystem.Init(initSO.assetLoadMode);
            await GlobalConfig.Instance.Init(initSO);
            TcpManager.Instance.Init();
            OnInitCompleted?.Invoke();
        }

        public void AddInitCompletedEvent(Action action)
        {
            OnInitCompleted += action;
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
