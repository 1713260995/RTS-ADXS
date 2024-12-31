namespace Assets.Scripts.Modules.AI
{
    public class AttackAI : IAttackAI
    {
        protected GameRoleCtrl role { get; private set; }
        public GameUnitCtrl currentTarget { get; private set; }

        public bool IsAlive => currentTarget != null;

        public AttackAI(GameRoleCtrl role)
        {
            this.role = role;
            currentTarget = null;
        }

        public void OnAttack(GameUnitCtrl target)
        {
            if (!role.CanAttack(target))
            {
                return;
            }
            currentTarget = target;
            role.followAI.OnFollow(new FollowInfo(currentTarget, role.attackDistance, () => { StartAttack(target); }));//追踪目标直至到达攻击距离
        }

        protected void StartAttack(GameUnitCtrl target)
        {
            role.stateMachine.TryTrigger(StateName.Attack);
            currentTarget = null;
        }

        public void AbortAI()
        {
            if (currentTarget != null)
            {
                if (role.currentState == StateName.Move)
                {
                    //如果当前运行攻击AI，并在在追踪目标。则结束此任务
                    role.followAI.AbortAI();
                }
                role.stateMachine.TryTrigger(StateName.Idle);
                currentTarget = null;
            }
        }
    }
}