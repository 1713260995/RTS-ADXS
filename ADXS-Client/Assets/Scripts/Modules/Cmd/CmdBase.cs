using Assets.Scripts.Modules.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Cmd
{
    public abstract class CmdBase : ICmd
    {
        protected abstract StateName stateId { get; }

        protected GameRoleCtrl ctrl;

        protected RoleStateMachine stateMachine;

        public CmdBase(GameRoleCtrl _ctrl)
        {
            ctrl = _ctrl;
            stateMachine = ctrl.stateMachine;
        }


        public abstract bool Execute<T>(T obj);

        protected bool TryChangeState()
        {
            return stateMachine.TryTrigger(stateId);
        }
    }
}
