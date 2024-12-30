using Assets.GameClientLib.Scripts.Utils;
using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.FSM.Role;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

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
        InitCmd();
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
        var list = new List<State>()
        {
            new MoveState(),
            new IdleState(),
            new AttackState(),
        };
        return list;
    }

    #endregion

    #region Cmd

    protected virtual void InitCmd()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.angularSpeed = 0;
        navAgent.speed = moveSpeed;
    }

    #region Idle

    public void OnIdle()
    {
        stateMachine.TryTrigger(StateName.Idle);
    }

    #endregion

    #region Move

    public float maxMoveInterval = 0.5f;
    public float moveSpeed = 5;
    public float rotateLerp = 0.01f;

    public NavMeshAgent navAgent { get; private set; }
    public Vector3 currentEndPoint { get; private set; }
    protected Coroutine moveTask { get; private set; }



    public void OnMove(Vector3 endPoint, Action onComplete = null)
    {
        if (currentEndPoint == endPoint && currentState == StateName.Move)
            return;//如果更新目标点时，新目标点和当前移动的点位置一致，则不需要更新
        moveTask = StartCoroutine(Move(endPoint, onComplete));
    }

    protected virtual IEnumerator Move(Vector3 endPoint, Action onComplete)
    {
        currentEndPoint = endPoint;
        if (currentState != StateName.Move)
        {
            stateMachine.TryTrigger(StateName.Move);
        }
        navAgent.isStopped = false;
        navAgent.SetDestination(endPoint);
        while (currentEndPoint == endPoint)
        {
            var s = MyMath.LookAt(transform, endPoint);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, s, MyMath.GetLerp(rotateLerp));
            if ((endPoint - transform.position).magnitude <= maxMoveInterval && navAgent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                OnIdle();
                onComplete?.Invoke();
                Debug.Log("到达终点");
                break;
            }
            // transform.rotation=Quaternion.ler
            yield return null;
        }
        moveTask = null;
    }


    public IEnumerator Follow(GameUnitCtrl target, float stopDis, Action onComplete = null)
    {

        while ((transform.position - target.transform.position).magnitude > stopDis)
        {
            OnMove(target.transform.position, onComplete);
            yield return null;
        }
        StopMove();
    }

    [ShowButton]
    public void StopMove()
    {
        navAgent.isStopped = true;
        OnIdle();
    }

    #endregion

    #region Attack

    public float attackDistance = 1.5f;
    public bool isAttacking { get; set; }//正在攻击
    public bool isAttackJudge { get; private set; }//正在进行攻击判定
    protected Coroutine currentAttackTask { get; private set; }

    public void OnAttack(GameUnitCtrl target)
    {
        if (!CanAttack(target))
            return;
        if (currentAttackTask != null)
            StopCoroutine(currentAttackTask);
        currentAttackTask = StartCoroutine(Attack(target));

    }

    protected IEnumerator Attack(GameUnitCtrl target)
    {
        yield return StartCoroutine(Follow(target, attackDistance));
        stateMachine.TryTrigger(currentState, StateName.Attack);
        currentAttackTask = null;
    }

    /// <summary>
    /// 攻击判定开始
    /// </summary>
    public void AttackJudgeStart()
    {
        isAttackJudge = true;
    }

    /// <summary>
    /// 攻击判定结束
    /// </summary>
    public void AttackJudgetDone()
    {
        isAttackJudge = false;
    }

    public void AttackDone()
    {
        OnIdle();
    }

    #endregion

    #endregion
}
