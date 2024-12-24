using Assets.Scripts.Common.Enum;
using System;

namespace Assets.Scripts.Modules.FSM.Role
{
    internal class AttackState : RoleStateBase
    {
        public override StateName stateId => StateName.Attack;

        protected override StateName[] nextStates => new StateName[] { StateName.Idle };

        protected override RoleAnimFlags animName => RoleAnimFlags.Attack;

        public override void OnEnter()
        {
            anim.SetTrigger(animNameHash);
            role.isAttacking = true;
        }

        public override void OnExit()
        {
            role.isAttacking = false;
        }

        public override void OnUpdate()
        {

        }
    }
}
