using Assets.Scripts.Modules.Cmd;
using Assets.Scripts.Modules.FSM;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.Command
{
    public class MoveCmdByNav : MoveCmdBase
    {
        private NavMeshAgent agent;

        public MoveCmdByNav(GameRoleCtrl _ctrl) : base(_ctrl)
        {
            agent = _ctrl.GetComponent<NavMeshAgent>();
        }


        public override bool Move(Vector3 point)
        {
            bool result = true;
            if (ctrl.currentState == StateName.Move || TryChangeState())
            {
                agent.SetDestination(point);
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
