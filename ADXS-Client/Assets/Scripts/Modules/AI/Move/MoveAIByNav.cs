using Assets.GameClientLib.Scripts;
using Assets.GameClientLib.Scripts.Utils;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIByNav : MoveAIBase, IMoveAI
    {
        protected NavMeshAgent navAgent { get; set; }
        public override bool IsAlive => moveTask != null;
        public Vector3 currentDestination;


        public MoveAIByNav(GameRoleCtrl role) : base(role)
        {
            navAgent = role.GetComponent<NavMeshAgent>();
            navAgent.angularSpeed = 0;//禁止nav自带旋转
        }

        public override void OnMove(IMoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            navAgent.SetDestination(moveInfo.Destination);
            currentDestination = moveInfo.Destination;
            navAgent.speed = role.MoveSpeed;
            if (!IsAlive)
            {
                role.stateMachine.TryTrigger(StateName.Move);
                navAgent.isStopped = false;
                moveTask = role.StartCoroutine(Move());
            }
        }

        protected override void UpdatePosAndDir()
        {
            Vector3 rotateEuler = MyMath.LookAt(transform, moveInfo.Destination);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateEuler, MyMath.GetLerp(role.rotateLerp));
            if (currentDestination != moveInfo.Destination)
            {
                //终点可能会不停变化，所以如果检查到当前终点有变化就重新设置终点
                navAgent.SetDestination(moveInfo.Destination);
                currentDestination = moveInfo.Destination;
            }
        }


        public override void AbortAI()
        {
            base.AbortAI();
            if (IsAlive)
            {
                navAgent.isStopped = true;
            }
        }
    }
}
