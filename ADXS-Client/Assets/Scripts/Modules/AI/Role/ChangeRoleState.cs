using Assets.Scripts.Common.Enum;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Role
{
    [TaskCategory(BTShareStr.taskCategoryRole)]
    [TaskDescription("将用户当前状态改为目标状态")]
    public class ChangeRoleState : Action
    {
        private GameRoleCtrl roleCtrl;
        private SharedRoleState share;

        public override void OnAwake()
        {
            roleCtrl = GetComponent<GameRoleCtrl>();
            share = Owner.GetVariable(BTShareStr.roleNextState) as SharedRoleState;
        }


        public override void OnEnd()
        {
            RoleState nextState = (RoleState)share.GetValue();
            roleCtrl.currentState = nextState;
            share.SetValue(RoleState.Unknow);
            Debug.Log("ChangeRoleState：" + roleCtrl.currentState);
        }

    }
}
