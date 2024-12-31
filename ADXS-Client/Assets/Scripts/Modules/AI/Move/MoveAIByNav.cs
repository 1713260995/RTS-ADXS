using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Utils;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.AI.Move
{
    public class MoveAIByNav : IMoveAI
    {
        protected GameRoleCtrl role { get; set; }
        protected Transform transform { get; set; }
        protected NavMeshAgent navAgent { get; set; }
        protected Coroutine moveTask { get; set; }
        protected MoveInfo moveInfo { get; set; }


        public MoveAIByNav(GameRoleCtrl role)
        {
            this.role = role;
            transform = role.transform;
            navAgent = role.GetComponent<NavMeshAgent>();
            navAgent.angularSpeed = 0;//禁止nav自带旋转
        }

        public bool IsAlive => moveTask != null;

        public void OnMove(MoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            navAgent.SetDestination(moveInfo.endPoint);
            navAgent.speed = role.moveSpeed;
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
            navAgent.isStopped = true;
            moveInfo.onComplete?.Invoke();
            role.idleAI.OnIdle();
            moveTask = null;
            Debug.Log("到达终点");
        }


        public void AbortAI()
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
