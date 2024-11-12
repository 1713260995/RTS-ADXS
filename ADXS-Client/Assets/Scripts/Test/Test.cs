using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Role;
using Assets.Scripts.Modules.Spawn;
using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Test : MonoBehaviour
{

    public SpawnSystem spawnSystem;

    private IObjectPool<GameRoleCtrl> objectPool;
    public GameRoleCtrl role;


    [ShowButton]
    public void CreateFarmer()
    {
        objectPool = spawnSystem.GetUnitPool<GameRoleCtrl>(GameUnitName.Peasant);
        role = objectPool.Get();
    }


    [ShowButton]
    public void IdleToMove()
    {
        role.stateMachine.TryTrigger(StateName.Idle, StateName.Move).Forget();
    }

    [ShowButton]
    public void MoveToIdle()
    {
        role.stateMachine.TryTrigger(StateName.Move, StateName.Idle).Forget();
    }
}
