using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI.Idel
{
    public class IdleAI : IIdleAI
    {
        protected GameRoleCtrl role;
        public IdleAI(GameRoleCtrl role)
        {
            this.role = role;
        }

        public void OnIdle()
        {
            role.stateMachine.TryTrigger(StateName.Idle);
        }
    }
}
