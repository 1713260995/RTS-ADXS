using Assets.Scripts.Common.Enum;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Role
{
    public class ChangeRoleState : BehaviorDesigner.Runtime.Tasks.Action
    {
        public GameRoleCtrl roleCtrl;



        public override void OnAwake()
        {
            base.OnAwake();
            roleCtrl = GetComponent<GameRoleCtrl>();
        }

        public override void OnEnd()
        {
            RoleState nextState = (RoleState)((SharedRoleState)Owner.GetVariable(BTGlobeVariable.roleNextState)).GetValue();
            roleCtrl.currentState = nextState;
            Debug.Log("用户当前状态：=" + roleCtrl.currentState);
        }
    }
}
