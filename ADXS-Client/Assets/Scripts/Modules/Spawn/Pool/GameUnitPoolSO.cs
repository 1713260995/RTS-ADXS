using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Resource.Pool;
using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Role;
using Assets.Scripts.Modules.Spawn;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameUnitPoolSO<TCtrl> : ObjectPoolSO<TCtrl>, ISpwanUnit where TCtrl : GameUnitCtrl
{
    public AssetReference factoryReference;
    public GameUnitName unitName;
    protected IFactory<TCtrl> factory;
    public GameUnitName spwanUnit => unitName;

    public void OnEnable()
    {
        LoadFactory();
    }


    protected override TCtrl CreatePooledItem()
    {
        return factory.Create();
    }


    protected override void OnGetFromPool(TCtrl item)
    {
        item.gameObject.SetActive(true);

    }

    protected override void OnReturnedToPool(TCtrl item)
    {
        item.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(TCtrl item)
    {
        Debug.Log("调销毁了");
        Destroy(item.gameObject);
    }

    [ShowButton]
    protected void LoadFactory()
    {
        if (factory != null)
        {
            return;
        }
        if (factoryReference.IsNull())
        {
            Debug.LogError($"{name} reference is null");
            return;
        }
        factory = ResSystem.Load<FactorySO<TCtrl>>(factoryReference);
    }
}
