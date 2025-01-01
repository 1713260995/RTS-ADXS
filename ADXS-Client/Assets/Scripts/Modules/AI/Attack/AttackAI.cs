namespace Assets.Scripts.Modules.AI
{
    public class AttackAI : AIBase, IAttackAI
    {
        public GameUnitCtrl currentTarget { get; private set; }

        public override bool IsAlive => currentTarget != null;

        public AttackAI(GameRoleCtrl role) : base(role)
        {
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


        //如果当前处于跟随状态，会自动切换至idle状态
        //如果当前处于攻击状态，会在攻击完成后自动切换至idle状态
        public override void AbortAI()
        {
            if (currentTarget != null)
            {
                if (role.currentState == StateName.Move)
                {
                    //如果当前运行攻击AI，并在在追踪目标。则结束此任务
                    role.followAI.AbortAI();
                }
                currentTarget = null;
            }
        }
    }
}