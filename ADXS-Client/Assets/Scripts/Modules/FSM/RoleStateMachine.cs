using Assets.GameClientLib.Scripts.Utils.FSM;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleStateMachine : StateMachine
    {
        public GameRoleCtrl ctrl { get; private set; }

        public RoleStateMachine(GameRoleCtrl _ctrl)
        {
            ctrl = _ctrl;
        }

        public StateName GetCurrentStateName()
        {
            var s = currentState as RoleStateBase;
            return s.stateId;
        }

        public bool TryTrigger(StateName target)
        {
            return TryTrigger(ctrl.currentState, target);
        }

        public bool TryTrigger(StateName origin, StateName target)
        {
            return TryTrigger(RoleTransition.GenerateId(origin, target));
        }

        public bool CanTransition(StateName origin, StateName target)
        {
            return CanTransition(RoleTransition.GenerateId(origin, target));
        }

        public bool IsCurrentStateOnAnimation(StateName stateName, int layerIndex = 0)
        {
            AnimatorStateInfo currentStateInfo = ctrl.animator.GetCurrentAnimatorStateInfo(layerIndex);
            return currentStateInfo.IsName(stateName.ToString());
        }


        private bool IsInSubStateMachine(AnimatorStateInfo stateInfo, out string subStateMachineName)
        {
            subStateMachineName = null;

            // 获取 Animator Controller
            var runtimeController = ctrl.animator.runtimeAnimatorController as AnimatorController;
            if (runtimeController == null) return false;

            // 遍历 Animator 的所有层
            foreach (var layer in runtimeController.layers)
            {
                foreach (var stateMachine in layer.stateMachine.stateMachines)
                {
                    if (stateMachine.stateMachine.states.Length > 0)
                    {
                        foreach (var subState in stateMachine.stateMachine.states)
                        {
                            if (subState.state.nameHash == stateInfo.shortNameHash)
                            {
                                subStateMachineName = stateMachine.stateMachine.name;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

    }
}
