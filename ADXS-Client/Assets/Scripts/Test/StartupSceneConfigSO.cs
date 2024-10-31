using Assets.GameClientLib.Utils.ScriptableObjects;
using Assets.Scripts.Common.Enum;
using UnityEngine;

namespace Assets.GameClientLib.Scripts.Config.SO
{
    [CreateAssetMenu(fileName = "StartupScene", menuName = "ScriptableObject/Config/StartupScene", order = 0)]
    public class StartupSceneConfigSO : ConfigBaseSO
    {
        public SceneName sceneName = SceneName.Startup;
    }
}
