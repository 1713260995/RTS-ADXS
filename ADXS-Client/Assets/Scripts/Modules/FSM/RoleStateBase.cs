using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleStateBase : State
    {
        public override string id => stateName.ToString();

        protected readonly RoleState stateName;

        protected RoleStateMachine roleSM { get; private set; }
        protected Animator anim => roleSM.ctrl.GetComponent<Animator>();

        private int stateHash;

        public RoleStateBase(RoleState _stateName)
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

        public static RoleStateBase QuickCreate(RoleState orgin, List<RoleState> targets)
        {
            RoleStateBase state = new RoleStateBase(orgin);
            foreach (var target in targets)
            {
                RoleTransition roleTransition = new RoleTransition(orgin, target);
                state.AddTransition(roleTransition);
            }
            return state;
        }
    }
}
