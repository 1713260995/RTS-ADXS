using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Spawn;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class BattleSceneCtrl : MonoBehaviour
    {

        [SerializeField] private BattleSystem battleSystemPrefab;

        [SerializeField] private SpawnSystem spawnSystemPrefab;

        private void Awake()
        {
            Instantiate(battleSystemPrefab);
            Instantiate(spawnSystemPrefab);
        }


        private void OnDestroy()
        {

        }
    }
}
