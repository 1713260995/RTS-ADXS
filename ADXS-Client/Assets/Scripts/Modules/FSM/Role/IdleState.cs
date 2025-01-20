using Assets.Scripts.Common.Enum;
using System;

namespace Assets.Scripts.Modules.FSM.Role
{
    internal class IdleState : RoleStateBase
    {
        public override StateName stateId => StateName.Idle;

        protected override RoleAnimFlags animName => RoleAnimFlags.Idle;

        protected override StateName[] InitNextState() => new StateName[] { StateName.Move, StateName.Attack, StateName.Build };

        public override void OnEnter()
        {
            anim.SetBool(animNameHash, true);
        }

        public override void OnExit()
        {
            anim.SetBool(animNameHash, false);
        }

        public override void OnUpdate()
        {

        }
    }
}
