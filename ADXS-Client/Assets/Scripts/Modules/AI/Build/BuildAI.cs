using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.Role;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BuildAI : AIBase, IBuildAI
{

    public override bool IsAlive => isAlive;

    public GameBuildingCtrl CurrentBuilding => currentBuilding;
    protected bool isAlive;
    protected IMoveAI moveAI;
    protected GameBuildingCtrl currentBuilding;
    protected FarmerCtrl farmer;

    public BuildAI(GameRoleCtrl role, IMoveAI moveAI) : base(role)
    {
        farmer = role as FarmerCtrl;
        this.moveAI = moveAI;
    }

    public override void AbortAI()
    {
    }

    public void OnBuild(BuildInfo info)
    {
        currentBuilding = info.building;
        isAlive = true;
        IMoveInfo moveInfo = new MoveInfoByObj(info.building.gameObject, () => ArriveWay.IsArriveByRaycast(role, currentBuilding, farmer.buildDistance), () => StartBuild(info));
        moveAI.OnMove(moveInfo);
    }


    private void StartBuild(BuildInfo info)
    {
        isAlive = false;
        role.stateMachine.TryTrigger(StateName.Build);
        info.OnExecuteBuild(OnBuildComplete);
    }

    private void OnBuildComplete()
    {
        farmer.OnIdle();
        currentBuilding = null;
    }
}
