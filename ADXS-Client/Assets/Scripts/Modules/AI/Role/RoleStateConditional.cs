using Assets.Scripts.Common.Enum;
using BehaviorDesigner.Runtime.Tasks;

namespace Assets.Scripts.Modules.AI.Role
{
    [TaskCategory(BTShareStr.taskCategoryRole)]
    public class RoleStateConditional : Conditional
    {
        public StateName targetState;

        private GameRoleCtrl roleCtrl;

        public override void OnAwake()
        {
            roleCtrl = GetComponent<GameRoleCtrl>();
        }

        public override TaskStatus OnUpdate()
        {
            if (roleCtrl.currentState == targetState)
            {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

    }
}
