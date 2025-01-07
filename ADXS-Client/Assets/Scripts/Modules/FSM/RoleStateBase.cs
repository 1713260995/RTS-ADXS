using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM
{
    public abstract class RoleStateBase : State
    {
        public override string id { get; }
        public abstract StateName stateId { get; }
        protected StateName[] nextStates { get; }
        protected abstract RoleAnimFlags animName { get; }
        protected int animNameHash { get; }
        protected Animator anim { get; private set; }
        public GameRoleCtrl role { get; private set; }

        public RoleStateBase()
        {
            id = stateId.ToString();
            animNameHash = Animator.StringToHash(animName.ToString());
            nextStates = InitNextState();
            GenerateTransitions();
        }

        protected abstract StateName[] InitNextState();

        protected virtual void GenerateTransitions()
        {
            foreach (var next in nextStates)
            {
                RoleTransition transition = new RoleTransition(stateId, next);
                AddTransition(transition);
            }
        }

        public override void SetStateMachine(StateMachine _stateMachine)
        {
            base.SetStateMachine(_stateMachine);
            var roleSM = stateMachine as RoleStateMachine;
            role = roleSM.ctrl;
            anim = roleSM.ctrl.animator;
        }

        public RoleTransition FindTransition(StateName next)
        {
            RoleTransition roleTran = (RoleTransition)transitions.First(o => o.Id == RoleTransition.GenerateId(stateId, next));
            return roleTran;
        }

    }
}
