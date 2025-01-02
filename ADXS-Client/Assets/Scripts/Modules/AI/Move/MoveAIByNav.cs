using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Utils;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIByNav : AIBase, IMoveAI
    {
        protected NavMeshAgent navAgent { get; set; }
        protected Coroutine moveTask { get; set; }
        protected MoveInfo moveInfo { get; set; }
        public override bool IsAlive => moveTask != null;

        public MoveAIByNav(GameRoleCtrl role) : base(role)
        {
            navAgent = role.GetComponent<NavMeshAgent>();
            navAgent.angularSpeed = 0;//禁止nav自带旋转
        }

        public void OnMove(MoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            navAgent.SetDestination(moveInfo.endPoint);
            navAgent.speed = role.MoveSpeed;
            if (!IsAlive)
            {
                role.stateMachine.TryTrigger(StateName.Move);
                navAgent.isStopped = false;
                moveTask = role.StartCoroutine(Move());
            }
        }

        protected IEnumerator Move()
        {
            while ((moveInfo.endPoint - transform.position).magnitude > moveInfo.moveStopDis)
            {
                Vector3 rotateEuler = MyMath.LookAt(transform, moveInfo.endPoint);
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateEuler, MyMath.GetLerp(role.rotateLerp));
                yield return null;
            }
            moveInfo.onComplete?.Invoke();
            Debug.Log("到达终点");
            AbortAI();
        }

        public override void AbortAI()
        {
            if (IsAlive)
            {
                navAgent.isStopped = true;
                role.StopCoroutine(moveTask);
                moveTask = null;
                role.idleAI.OnIdle();
            }
        }
    }
}
