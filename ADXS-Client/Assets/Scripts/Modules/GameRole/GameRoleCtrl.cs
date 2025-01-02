using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.AI.Follow;
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


    protected override void Awake()
    {
        base.Awake();
        InitFSM();
        InitAI();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.OnUpdate();
    }

    #region State

    [HideInInspector]
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
        var list = new List<State>() {
            new MoveState(),
            new IdleState(),
            new AttackState(),
        };
        return list;
    }

    #endregion

    #region AI

    public List<IAIBase> aIBases = new List<IAIBase>();

    public IIdleAI idleAI { get; protected set; }
    public IMoveAI moveAI { get; protected set; }
    public IAttackAI attackAI { get; protected set; }
    public IFollowAI followAI { get; protected set; }

    protected virtual void InitAI()
    {
        attackAI = new AttackAI(this);
        idleAI = new IdleAI(this);
        moveAI = new MoveAIByORCA(this);
        followAI = new FollowAI(this);
    }

    #endregion

    #region Idle

    //待添加参数

    #endregion

    #region Move

    public float moveStopDis = 0.5f;

    [SerializeField]
    protected float moveSpeed = 5;

    public float rotateLerp = 0.01f;

    public float MoveSpeed => GetRealMoveSpeed();

    protected virtual float GetRealMoveSpeed()
    {
        return moveSpeed;
    }

    #endregion

    #region Attack

    public float attackDistance = 1.5f;
    public bool isAttacking { get; set; } //正在攻击
    public bool isAttackJudge { get; private set; } //正在进行攻击判定

    /// <summary>
    /// 攻击判定开始
    /// </summary>
    protected void AttackJudgeStart()
    {
        isAttackJudge = true;
    }

    /// <summary>
    /// 攻击判定结束
    /// </summary>
    protected void AttackJudgeDone()
    {
        isAttackJudge = false;
    }

    /// <summary>
    /// 攻击动作结束
    /// </summary>
    protected void AttackDone()
    {
        idleAI.OnIdle();
    }

    #endregion
}