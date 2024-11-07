using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Modules.Role
{
    public class GameUnitFactorySO<TCtrl, TEntity> : FactorySO<TCtrl>
        where TCtrl : GameUnitCtrl
        where TEntity : GameUnit, new()
    {
        public GameUnitName unitName;
        public GameUnitType unitType;

        public AssetReference reference;

        private GameObject prefab;

        private void OnEnable()
        {
            LoadPrebfab();
        }

        public override TCtrl Create()
        {
            LoadPrebfab();
            GameObject obj = Instantiate(prefab);
            TCtrl ctrl = obj.AddComponent<TCtrl>();
            TEntity entity = CreateEnity();
            ctrl.Init(entity);
            return ctrl;
        }

        protected virtual TEntity CreateEnity()
        {
            TEntity entity = new TEntity();
            entity.unitName = unitName;
            entity.unitType = unitType;
            return entity;
        }

        protected void LoadPrebfab()
        {
            if (prefab != null)
            {
                return;
            }
            if (reference.IsNull())
            {
                Debug.LogError($"{name} reference is null");
                return;
            }
            prefab = ResSystem.Load<GameObject>(reference);
        }


    }
}
