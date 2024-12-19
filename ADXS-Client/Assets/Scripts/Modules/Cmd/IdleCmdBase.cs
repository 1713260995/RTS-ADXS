using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.Cmd
{
    internal class IdleCmdBase : CmdBase
    {

        protected override StateName stateId => StateName.Idle;

        public IdleCmdBase(GameRoleCtrl _ctrl) : base(_ctrl)
        {
        }

        public override bool Execute<T>(T obj)
        {
            return TryChangeState();
        }
    }
}
