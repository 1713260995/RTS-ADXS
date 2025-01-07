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

        public MoveAIByNav(GameRoleCtrl role) : base(role)
        {
            navAgent = role.GetComponent<NavMeshAgent>();
            navAgent.angularSpeed = 0;//禁止nav自带旋转
        }

        public override void OnMove(MoveInfo _moveInfo)
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
