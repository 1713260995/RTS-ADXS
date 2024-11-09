using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Spawn;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts.Modules.Role
{
    public class GameUnitFactorySO<TCtrl, TEnity> : FactorySO<TCtrl>, ISpwanUnit where TCtrl : GameUnitCtrl where TEnity : GameUnit, new()
    {
        public GameUnitName unitName;
        public GameUnitType unitType;

        public AssetReference reference;

        protected GameObject prefab;

        public GameUnitName spwanUnit => unitName;

        private void OnEnable()
        {
            LoadPrebfab();
        }



        private void OnDisable()
        {
            prefab = null;
        }

        public override TCtrl Create()
        {
            LoadPrebfab();
            GameObject obj = Instantiate(prefab);
            TCtrl ctrl = obj.AddComponent<TCtrl>();
            GameUnit entity = CreateEnity();
            ctrl.Init(entity);
            return ctrl;
        }

        protected virtual TEnity CreateEnity()
        {
            TEnity entity = new TEnity();
            entity.unitName = unitName;
            entity.unitType = unitType;
            return entity;
        }

        protected void LoadPrebfab()
        {
            if (reference.IsNull())
            {
                throw new ArgumentNullException($"{name} reference is null");
            }

            if (prefab != null)
            {
                return;
            }

            prefab = ResSystem.Load<GameObject>(reference);
        }


    }
}
