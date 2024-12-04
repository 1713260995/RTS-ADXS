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

        public RoleStateMachine(List<State> stateList, RoleState defaultStateId, GameRoleCtrl _ctrl) : base(stateList, defaultStateId.ToString())
        {
            ctrl = _ctrl;
        }

        public async UniTask<bool> TryTrigger(RoleState origin, RoleState target)
        {
            return await TryTrigger(RoleTransition.GenerateId(origin, target));
        }

    }
}
