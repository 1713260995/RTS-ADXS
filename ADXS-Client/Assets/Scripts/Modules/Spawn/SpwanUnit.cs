using Assets.GameClientLib.Scripts.Utils.Factory;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules.Spawn
{

    public class SpwanUnit : IFactory<GameUnitCtrl>
    {
        private GameUnitCtrl prefab;

        public SpwanUnit(GameUnitCtrl prefab)
        {
            this.prefab = prefab;
        }

        public virtual GameUnitCtrl Create()
        {
            return UnityEngine.Object.Instantiate(prefab).GetComponent<GameUnitCtrl>();
        }

        public virtual void Destory(GameUnitCtrl ctrl)
        {
            UnityEngine.Object.Destroy(ctrl.gameObject);
        }
    }
}
