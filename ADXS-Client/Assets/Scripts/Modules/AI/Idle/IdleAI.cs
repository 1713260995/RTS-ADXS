using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class IdleAI : AIBase, IIdleAI
    {
        public IdleAI(GameRoleCtrl role) : base(role) { }

        public override bool IsAlive => role.currentState == StateName.Idle;

        /// <summary>
        /// Idle中止无需操作
        /// </summary>
        public override void AbortAI()
        {

        }

        public void OnIdle()
        {
            if (IsAlive)
            {
                return;
            }
            role.stateMachine.TryTrigger(StateName.Idle);
        }
    }
}
