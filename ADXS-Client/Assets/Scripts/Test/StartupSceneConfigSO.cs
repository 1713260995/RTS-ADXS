using Assets.GameClientLib.Utils.ScriptableObjects;
using Assets.Scripts.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameClientLib.Scripts.Config.SO
{
    [CreateAssetMenu(fileName = "StartupScene", menuName = "ScriptableObject/Config/StartupScene", order = 0)]
    public class StartupSceneConfigSO : ConfigBaseSO
    {
        public SceneName sceneName = SceneName.Startup;
    }
}
