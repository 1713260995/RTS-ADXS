using Assets.Scripts.Common.Enum;
using System;

namespace Assets.Scripts.Modules.FSM.Role
{
    internal class AttackState : RoleStateBase
    {
        public override StateName stateId => StateName.Attack;

        protected override RoleAnimFlags animName => RoleAnimFlags.Attack;

        protected override StateName[] InitNextState() => new StateName[] { StateName.Idle };

        public override void OnEnter()
        {
            role.isAttacking = true;
            anim.SetTrigger(animNameHash);
        }

        public override void OnExit()
        {

        }

        public override void OnUpdate()
        {

        }
    }
}
