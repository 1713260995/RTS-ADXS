using Assets.Scripts.Modules.FSM;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Modules.Command
{
    public class AttackCmdBase : ICmd
    {
        private GameRoleCtrl ctrl;

        public AttackCmdBase(GameRoleCtrl _ctrl)
        {
            ctrl = _ctrl;
        }

        public bool Execute<T>(T obj)
        {
            Debug.Log("攻击");
            return ctrl.stateMachine.TryTrigger(ctrl.currentState, StateName.Attack);
        }
    }
}
