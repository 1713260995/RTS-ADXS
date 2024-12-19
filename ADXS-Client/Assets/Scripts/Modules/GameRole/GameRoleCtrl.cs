using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.Cmd;
using Assets.Scripts.Modules.Command;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.FSM.Role;
using Assets.Scripts.Modules.Role;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameRoleCtrl : GameUnitCtrl
{
    public RoleType roleType;
    public RaceFlags raceType;

    public List<BuffBase> buffList { get; private set; }
    public RoleAttributes roleAttributes { get; private set; }
    public Animator animator { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        stateMachine = new RoleStateMachine(InitRoleStates(), defaultState, this);
        stateMachine.Restart();
        Commands = InitCmd();
    }

    #region State

    public StateName currentState => stateMachine.GetCurrentStateName();
    public RoleStateMachine stateMachine { get; private set; }
    protected virtual StateName defaultState => StateName.Idle;
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
    public virtual Dictionary<CommandFalgs, ICmd> Commands { get; private set; }

    protected virtual Dictionary<CommandFalgs, ICmd> InitCmd()
    {
        var cmds = new Dictionary<CommandFalgs, ICmd> {
            { CommandFalgs.Idle,new IdleCmdBase(this)},
            { CommandFalgs.Move,new MoveCmdByNav(this)},
           // { CommandFalgs.Attack,new AttackCmdBase(this) },
        };
        return cmds;
    }

    /// <summary>
    /// 执行指定命令
    /// </summary>
    public bool ExecuteTargetCmd<T>(CommandFalgs falgs, T arg)
    {
        if (Commands.TryGetValue(falgs, out ICmd cmd))
        {
            return cmd.Execute(arg);
        }
        return false;
    }

    /// <summary>
    /// 根据参数类型，自动判断需要执行哪个命令
    /// </summary>
    public virtual bool AutoExecuteCmd<T>(T obj)
    {
        bool result = false;
        if (obj is Vector3 point)
        {
            result = ExecuteTargetCmd(CommandFalgs.Move, obj);
        }
        else if (obj is GameUnitCtrl unit && CanAttack(unit))
        {
            ExecuteTargetCmd(CommandFalgs.Attack, obj);
        }
        return result;
    }

    #endregion
}
