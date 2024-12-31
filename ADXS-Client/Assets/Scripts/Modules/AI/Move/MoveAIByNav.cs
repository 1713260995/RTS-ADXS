using Assets.GameClientLib.Scripts.Utils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.AI.Move
{
    public class MoveAIByNav : IMoveAI
    {
        protected GameRoleCtrl role { get; set; }
        protected Vector3 currentEndPoint { get; set; }
        protected Coroutine moveTask { get; set; }
        protected NavMeshAgent navAgent { get; set; }
        protected Transform transform { get; set; }

        public MoveAIByNav(GameRoleCtrl role)
        {
            this.role = role;
            transform = role.transform;
            navAgent = role.GetComponent<NavMeshAgent>();
            navAgent.angularSpeed = 0;
            navAgent.speed = role.moveSpeed;

        }

        public void OnMove(Vector3 endPoint, Action onComplete = null)
        {
            moveTask = role.StartCoroutine(Move(endPoint, onComplete));
        }

        protected IEnumerator Move(Vector3 endPoint, Action onComplete)
        {
            if (currentEndPoint == endPoint && role.currentState == StateName.Move)
            {
                yield break;//如果更新目标点时，新目标点和当前移动的点位置一致，则不需要更新
            }
            if (role.currentState != StateName.Move)
            {
                role.stateMachine.TryTrigger(StateName.Move);
            }
            currentEndPoint = endPoint;
            navAgent.isStopped = false;
            navAgent.SetDestination(endPoint);
            while (currentEndPoint == endPoint)
            {

                var s = MyMath.LookAt(transform, endPoint);
                transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, s, MyMath.GetLerp(role.rotateLerp));
                if ((endPoint - transform.position).magnitude <= role.maxMoveInterval && navAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    role.idleAI.OnIdle();
                    onComplete?.Invoke();
                    Debug.Log("到达终点");
                    break;
                }
                yield return null;
            }
            moveTask = null;
        }

        public void OnFollow(GameUnitCtrl target, float stopDis, Action onComplete = null)
        {
            moveTask = role.StartCoroutine(Follow(target, stopDis, onComplete));
        }

        protected IEnumerator Follow(GameUnitCtrl target, float stopDis, Action onComplete = null)
        {
            while ((transform.position - target.transform.position).magnitude > stopDis)
            {
                OnMove(target.transform.position, onComplete);
                yield return null;
            }
            Debug.Log("跟随结束");
            AbortTask();
        }


        public void AbortTask()
        {
            if (moveTask != null)
            {
                navAgent.isStopped = true;
                role.StopCoroutine(moveTask);
                moveTask = null;
                role.idleAI.OnIdle();
            }
        }
    }
}
