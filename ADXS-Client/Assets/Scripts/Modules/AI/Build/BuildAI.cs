using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.Role;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BuildAI : AIBase, IBuildAI
{

    public override bool IsAlive => isAlive;

    protected bool isAlive;

    protected IMoveAI moveAI;

    protected IIdleAI idleAI;

    protected FarmerCtrl farmer;

    public BuildAI(GameRoleCtrl role, IMoveAI moveAI, IIdleAI idleAI) : base(role)
    {
        farmer = role as FarmerCtrl;
        this.moveAI = moveAI;
        this.idleAI = idleAI;
    }

    public override void AbortAI()
    {
    }

    public void OnBuild(BuildInfo info)
    {
        isAlive = true;
        IMoveInfo moveInfo = new MoveInfoByObj(info.building.gameObject, () => IsArray(info.building), () => StartBuild(info));
        moveAI.OnMove(moveInfo);
    }

    private bool IsArray(GameBuildingCtrl building)
    {
        Transform t = farmer.transform;
        if (Physics.Raycast(t.position, t.forward, out RaycastHit hitInfo, Mathf.Max(5, farmer.buildDistance), GameLayerName.GameUnit.GetLayerMask()))
        {
            if (hitInfo.collider.gameObject.GetInstanceID() == building.gameObject.GetInstanceID())
            {
                if ((hitInfo.point - t.position).magnitude <= farmer.buildDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void StartBuild(BuildInfo info)
    {
        isAlive = false;
        role.stateMachine.TryTrigger(StateName.Idle);
        role.stateMachine.TryTrigger(StateName.Build);
        info.OnExecuteBuild(OnBuildComplete);
        s = Time.time;
    }
    public float s;

    private void OnBuildComplete()
    {
        farmer.OnIdle();
        Debug.Log("建筑完成,时间：" + (int)(Time.time - s));
    }
}
