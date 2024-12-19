using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Common.Enum;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules.FSM
{
    public class RoleStateMachine : StateMachine
    {
        public GameRoleCtrl ctrl { get; private set; }

        public RoleStateMachine(List<State> stateList, StateName defaultStateId, GameRoleCtrl _ctrl) : base(stateList, defaultStateId.ToString())
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

    }
}
