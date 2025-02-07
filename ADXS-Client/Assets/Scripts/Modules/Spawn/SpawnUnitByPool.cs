using System;
using UnityEngine.Pool;

namespace Assets.Scripts.Modules.Spawn
{
    public class SpawnUnitByPool : SpawnUnit
    {
        private IObjectPool<GameUnitCtrl> m_Pool;

        private ISpawnPool spawn;

        public SpawnUnitByPool(GameUnitCtrl prefab) : base(prefab)
        {
            m_Pool = new ObjectPool<GameUnitCtrl>(CreatePooledItem, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, maxSize: 10);
        }

        public override GameUnitCtrl Create()
        {
            return m_Pool.Get();
        }

        public override void Destroy(GameUnitCtrl ctrl)
        {
            m_Pool.Release(ctrl);
        }


        protected virtual GameUnitCtrl CreatePooledItem()
        {
            GameUnitCtrl item = base.Create();
            spawn = item as ISpawnPool;
            if (spawn == null)
            {
                throw new Exception("ISpawnPool Unrealized");
            }
            return item;
        }

        protected virtual void OnGetFromPool(GameUnitCtrl item)
        {
            spawn.GetFromPool();
        }

        protected virtual void OnReturnedToPool(GameUnitCtrl item)
        {
            spawn.ReturnedToPool();
        }

        protected virtual void OnDestroyPoolObject(GameUnitCtrl item)
        {
            base.Destroy(item);
        }
    }
}
