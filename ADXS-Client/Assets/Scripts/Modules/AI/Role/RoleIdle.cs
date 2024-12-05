using Assets.Scripts.Common.Enum;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Role
{
    [TaskCategory(BTShareStr.taskCategoryRole)]
    public class RoleIdle : Action
    {
        private int animHash;
        private Animator anim;
        private GameRoleCtrl roleCtrl;

        public override void OnAwake()
        {
            anim = GetComponent<Animator>();
            roleCtrl = GetComponent<GameRoleCtrl>();
            animHash = RoleAnimName.Idle.GetAnimHash();
        }

        public override void OnStart()
        {
            anim.SetBool(animHash, true);
        }

        public override TaskStatus OnUpdate()
        {
            if (roleCtrl.currentState == RoleState.Idle)
            {
                return TaskStatus.Running;
            }
            return TaskStatus.Success;
        }


        public override void OnEnd()
        {
            anim.SetBool(animHash, false);
        }
    }
}
