using Assets.GameClientLib.Resource;
using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Resource.Pool;
using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules.Role;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameUnitPoolSO<T> : ObjectPoolSO<T> where T : GameUnitCtrl
{
    public AssetReference factoryReference;
    public GameUnitName unitName;
    protected IFactory<T> factory;

    public void OnEnable()
    {
        LoadFactory();
    }


    protected override T CreatePooledItem()
    {
        return factory.Create();
    }


    protected override void OnGetFromPool(T item)
    {
        item.gameObject.SetActive(true);

    }

    protected override void OnReturnedToPool(T item)
    {
        item.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(T item)
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
        factory = ResSystem.Load<FactorySO<T>>(factoryReference);
    }
}
