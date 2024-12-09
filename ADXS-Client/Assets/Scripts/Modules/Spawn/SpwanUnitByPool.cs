
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System;
using UnityEngine.Pool;

namespace Assets.Scripts.Modules.Spawn
{
    public class SpwanUnitByPool : SpwanUnit
    {
        private IObjectPool<GameUnitCtrl> m_Pool;

        private ISpwanPool spwan;

        public SpwanUnitByPool(GameUnitCtrl prefab) : base(prefab)
        {
            m_Pool = new ObjectPool<GameUnitCtrl>(CreatePooledItem, OnGetFromPool, OnReturnedToPool, OnDestroyPoolObject, maxSize: 10);
        }

        public override GameUnitCtrl Create()
        {
            return m_Pool.Get();
        }

        public override void Destory(GameUnitCtrl ctrl)
        {
            m_Pool.Release(ctrl);
        }


        protected virtual GameUnitCtrl CreatePooledItem()
        {
            GameUnitCtrl item = base.Create();
            spwan = item as ISpwanPool;
            if (spwan == null)
            {
                throw new Exception("ISpwanPool Unrealized");
            }
            return item;
        }

        protected virtual void OnGetFromPool(GameUnitCtrl item)
        {
            spwan.GetFromPool();
        }

        protected virtual void OnReturnedToPool(GameUnitCtrl item)
        {
            spwan.ReturnedToPool();
        }

        protected virtual void OnDestroyPoolObject(GameUnitCtrl item)
        {
            base.Destory(item);
        }
    }
}
