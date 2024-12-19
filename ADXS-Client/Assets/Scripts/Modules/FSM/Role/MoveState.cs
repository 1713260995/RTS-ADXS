using Assets.Scripts.Common.Enum;

namespace Assets.Scripts.Modules.FSM
{
    internal class MoveState : RoleStateBase
    {
        public override StateName stateId => StateName.Move;
        protected override StateName[] nextStates => new StateName[] { StateName.Idle };
        protected override RoleAnimFlags animName => RoleAnimFlags.Walk;

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

        protected override void GenerateTransitions()
        {
            base.GenerateTransitions();
            RoleTransition idleTran = FindTransition(StateName.Idle);
            idleTran.canTransition = CanToIdle;
            idleTran.doTransition = ToIdle;
        }


        private void ToIdle()
        {

        }

        private bool CanToIdle()
        {
            return true;
        }
    }
}
