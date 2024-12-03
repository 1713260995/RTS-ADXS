using Assets.Scripts.Modules.Battle;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class BattleSceneCtrl : MonoBehaviour
    {

        public static BattleSystem battleSystem { get; private set; }

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            battleSystem = new BattleSystem();
        }
    }
}
