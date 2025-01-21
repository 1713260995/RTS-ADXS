using Assets.Scripts.Common.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM.Role
{
    internal class BuildState : RoleStateBase
    {
        public override StateName stateId => StateName.Build;

        protected override RoleAnimFlags animName => RoleAnimFlags.Build;

        protected float maxCoolingTime = 0.5f;
        protected float currentCoolingTime = 0;

        public override void OnEnter()
        {
            anim.SetTrigger(animNameHash);
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }

        protected override StateName[] InitNextState() => new StateName[] { StateName.Idle };

    }
}
