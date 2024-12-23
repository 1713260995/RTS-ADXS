using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.AI.FindWay;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.FSM.Role;
using System.Collections.Generic;
using UnityEngine;

public class GameRoleCtrl : GameUnitCtrl
{
    public RoleType roleType;
    public RaceFlags raceType;

    public List<BuffBase> buffList { get; private set; }
    public RoleAttributes roleAttributes { get; private set; }
    public Animator animator { get; private set; }


    protected override void OnAwake()
    {
        base.OnAwake();
        InitFSM();
        InitCmd();
    }


    #region State

    public StateName currentState => stateMachine.GetCurrentStateName();
    public RoleStateMachine stateMachine { get; private set; }
    protected virtual StateName defaultState => StateName.Idle;

    private void InitFSM()
    {
        animator = GetComponent<Animator>();
        stateMachine = new RoleStateMachine(this);
        stateMachine.Start(InitRoleStates(), defaultState.ToString());
    }

    protected virtual List<State> InitRoleStates()
    {
        var list = new List<State>()
        {
            new MoveState(),
            new IdleState(),
        };
        return list;
    }


    #endregion

    #region CMD

    protected IFindWay findWay { get; private set; }

    public void InitCmd()
    {
        findWay = new FindWayByNav(this);
    }

    public bool Attack(GameUnitCtrl target)
    {
        if (!CanAttack(target)) return false;

        return stateMachine.TryTrigger(currentState, StateName.Attack);
    }


    public void Move(Vector3 point)
    {
        if (currentState != StateName.Move)
        {
            stateMachine.TryTrigger(StateName.Move);
        }
        findWay.FindWay(point, Idle);
    }


    public void Idle()
    {
        stateMachine.TryTrigger(StateName.Idle);
    }



    #endregion
}
