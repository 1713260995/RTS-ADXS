using Assets.GameClientLib.Scripts.Utils.FSM;
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

        public async UniTask<bool> TryTrigger(StateName origin, StateName target)
        {
            return await TryTrigger(RoleTransition.GenerateId(origin, target));
        }

    }
}
