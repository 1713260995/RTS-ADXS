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
    public class SpawnSystem : SystemBase<SpawnSystem>
    {
        [SerializeField]
        private List<AssetReference> assetReferenceList;

        private Dictionary<GameUnitName, SpawnUnit> spawnDic { get; set; }

        protected override void Awake()
        {
            base.Awake();
            spawnDic = new();
            foreach (var item in assetReferenceList)
            {
                GameUnitCtrl ctrl = ResSystem.LoadAsset<GameObject>(item).GetComponent<GameUnitCtrl>();

                ISpawnPool spwanPool = ctrl as ISpawnPool;
                if (spwanPool != null)
                {
                    spawnDic.Add(ctrl.unitName, new SpawnUnitByPool(ctrl));
                }
                else
                {
                    spawnDic.Add(ctrl.unitName, new SpawnUnit(ctrl));
                }
            }
        }


        public TCtrl CreateCtrl<TCtrl>(GameUnitName _name) where TCtrl : GameUnitCtrl
        {
            var c = spawnDic[_name].Create();
            return c as TCtrl;
        }

        public void DestroyCtrl(GameUnitCtrl ctrl)
        {
            spawnDic[ctrl.unitName].Destroy(ctrl);
        }

        #region test
        [ReadOnlyField]
        public GameUnitName testName;

        private GameRoleCtrl testRole;

        [ShowButton]
        public void GetCtrl()
        {
            testRole = CreateCtrl<GameRoleCtrl>(testName);
        }

        [ShowButton]
        public void DestroyCtrl()
        {
            DestroyCtrl(testRole);
        }

        #endregion
    }
}
