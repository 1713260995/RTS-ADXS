using Assets.Scripts.Modules.Battle;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class BattleSceneCtrl : MonoBehaviour
    {
        [SerializeField]
        private BattleSystem battleSystemPrefab;

        private void Awake()
        {
            Instantiate(battleSystemPrefab);
        }


        private void OnDestroy()
        {

        }
    }
}
