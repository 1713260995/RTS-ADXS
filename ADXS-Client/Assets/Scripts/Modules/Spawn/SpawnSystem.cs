using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Assets.Scripts.Modules.Spawn
{
    public class SpawnSystem : MonoBehaviour
    {
        private Dictionary<GameUnitName, ISpwanUnit> spawnDic;

        [SerializeField]
        private List<AssetReference> assetReferenceList;

        private void Awake()
        {
            spawnDic = new Dictionary<GameUnitName, ISpwanUnit>();
            Init();
        }

        public void Init()
        {
            foreach (var item in assetReferenceList)
            {
                ScriptableObject so = ResSystem.Load<ScriptableObject>(item);

                if (so is ISpwanUnit pool)
                {
                    spawnDic.Add(pool.spwanUnit, pool);
                }
                else
                {
                    throw new ArgumentException("AssetReference is error");
                }
            }
        }

        public IFactory<TCtrl> GetUnitFactory<TCtrl>(GameUnitName unitName) where TCtrl : GameUnitCtrl
        {
            ISpwanUnit so = spawnDic[unitName];
            IFactory<TCtrl> factory = so as IFactory<TCtrl>;
            return factory;
        }

        public IObjectPool<TCtrl> GetUnitPool<TCtrl>(GameUnitName unitName) where TCtrl : GameUnitCtrl
        {
            ISpwanUnit so = spawnDic[unitName];
            GameUnitPoolSO<TCtrl> poolso = so as GameUnitPoolSO<TCtrl>;
            return poolso.GetPool();
        }


    }
}
