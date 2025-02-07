using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.FSM.Role;
using System.Collections.Generic;
using Modules.AI.Move;
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

    [ShowButton]
    private void TestDestroy()
    {
        Destroy(gameObject);
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
        stateMachine.OnStart(InitRoleStates(), defaultState.ToString());
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

    protected IIdleAI idleAI { get; set; }
    protected IMoveAI moveAI { get; set; }
    protected IAttackAI attackAI { get; set; }
    private IAIBase currentAI { get; set; }


    protected override void InitAI()
    {
        base.InitAI();
        idleAI = new IdleAI(this);
        // moveAI = new MoveAIBase(this);
        moveAI = new MoveAIByBoid(this);
        attackAI = new AttackAI(this, moveAI);
    }

    public void OnIdle()
    {
        if (SwitchCurrentAI(idleAI)) {
            idleAI.OnIdle();
        }
    }

    public void OnMove(IMoveInfo moveInfo)
    {
        if (SwitchCurrentAI(moveAI)) {
            moveAI.OnMove(moveInfo);
        }
    }

    public void OnAttack(GameUnitCtrl target)
    {
        if (SwitchCurrentAI(attackAI)) {
            attackAI.OnAttack(target);
        }
    }

    /// <summary>
    /// 切换AI
    /// </summary>
    /// <param name="ai"></param>
    private bool SwitchCurrentAI(IAIBase ai)
    {
        if (isAttacking) {
            return false;
        }

        if (ai != currentAI && currentAI != null) {
            currentAI.AbortAI();
        }

        currentAI = ai;
        return true;
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
        attackAI.AttackDone();
    }

    #endregion
}