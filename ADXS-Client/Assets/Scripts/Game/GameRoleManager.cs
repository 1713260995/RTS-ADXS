using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Resource.Pool;
using Assets.GameClientLib.Scripts.Utils.Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Assets.Scripts.Modules.Builder
{
    public class GameRoleManager : SingletonMono<GameRoleManager>
    {
        public IObjectPool<RoleBaseCtrl> objectPool;

        public AssetReference poolReference;


        [ShowButton]
        protected void LoadFactory()
        {
            if (objectPool != null)
            {
                return;
            }
            if (poolReference.IsNull())
            {
                Debug.LogError($"{name} reference is null");
                return;
            }
            objectPool = ResSystem.Load<ObjectPoolSO<RoleBaseCtrl>>(poolReference).GetPool();
        }



        List<RoleBaseCtrl> list = new();
        [ShowButton]
        public void Get()
        {
            LoadFactory();
            var ctrl = objectPool.Get();
            ctrl.transform.position = Random.insideUnitCircle;
            list.Add(ctrl);
        }
        [ShowButton]
        private void Release()
        {
            var ctrl = list.FirstOrDefault();
            if (ctrl != null)
            {
                objectPool.Release(ctrl);
                list.Remove(ctrl);
            }

        }

    }
}
