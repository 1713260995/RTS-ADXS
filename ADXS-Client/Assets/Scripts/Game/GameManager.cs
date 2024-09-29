using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts.Config;
using Assets.GameClientLib.Scripts.Config.SO;
using Assets.GameClientLib.Scripts.Utils.Singleton;
using Assets.Scripts.NetWork;
using Assets.Scripts.Scene;
using System;
using Cysharp.Threading.Tasks;

namespace Assets.GameClientLib.Scripts.Game
{
    public class GameManager : SingletonMono<GameManager>
    {
        public Action onInitCompleted { get; set; }

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
            InitConfigSO initSO = StartupSceneCtrl.initSO;
            ExceptionSystem.OnEnable();
            ResourceSystem.Init(initSO.assetLoadMode);
            await GlobalConfig.Instance.Init(initSO);
            TcpManager.Instance.Init();
            onInitCompleted?.Invoke();
        }
        #endregion
    }
}
