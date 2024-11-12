using Assets.GameClientLib.Scripts.Utils.FSM;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleState : State
    {
        public override string id => stateName.ToString();

        protected readonly StateName stateName;

        protected RoleStateMachine roleSM { get; private set; }
        protected Animator anim => roleSM.ctrl.animator;

        private int stateHash;

        public RoleState(StateName _stateName)
        {
            stateName = _stateName;

            stateHash = Animator.StringToHash(stateName.ToString());
        }

        public override void SetStateMachine(StateMachine _stateMachine)
        {
            base.SetStateMachine(_stateMachine);
            roleSM = stateMachine as RoleStateMachine;
        }


        public override void OnEnter()
        {
            anim.SetBool(stateHash, true);
        }

        public override void OnExit()
        {
            anim.SetBool(stateHash, false);
        }

        public override void OnUpdate()
        {

        }

        public static RoleState QuickCreate(StateName orgin, List<StateName> targets)
        {
            RoleState state = new RoleState(orgin);
            foreach (var target in targets)
            {
                RoleTransition roleTransition = new RoleTransition(orgin, target);
                state.AddTransition(roleTransition);
            }
            return state;
        }
    }
}
