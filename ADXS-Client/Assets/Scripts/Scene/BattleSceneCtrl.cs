using Assets.GameClientLib.Scripts.Config.SO;
using Assets.GameClientLib.Scripts.Game;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Battle;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class BattleSceneCtrl : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManagerPrefab;

        [SerializeField]
        private StartupSceneConfigSO startupSceneConfig;

        public static BattleSystem battleSystem { get; private set; }

        private void Awake()
        {
            if (startupSceneConfig.sceneName == SceneName.Battle)
            {
                GameManager manager = Instantiate(gameManagerPrefab);
                manager.AddInitCompletedEvent(Init);
            }
            else
            {
                Init();
            }
        }

        public void Init()
        {
            battleSystem = new BattleSystem();
        }
    }
}
