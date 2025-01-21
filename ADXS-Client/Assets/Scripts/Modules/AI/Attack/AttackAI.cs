using System.Data;

namespace Assets.Scripts.Modules.AI
{
    public class AttackAI : AIBase, IAttackAI
    {
        public GameUnitCtrl currentTarget { get; private set; }

        public override bool IsAlive => currentTarget != null;
        public IMoveAI moveAI { get; private set; }

        public AttackAI(GameRoleCtrl role, IMoveAI moveAI) : base(role)
        {
            currentTarget = null;
            this.moveAI = moveAI;
        }

        public void OnAttack(GameUnitCtrl target)
        {
            if (!role.CanAttack(target))
            {
                return;
            }

            currentTarget = target;
            //追踪目标直至到达攻击距离
            IMoveInfo moveInfo = new MoveInfoByObj(target.gameObject, () => ArriveWay.IsArriveByAttackDistance(role, target), StartAttack);
            moveAI.OnMove(moveInfo);

        }



        private void StartAttack()
        {
            role.stateMachine.TryTrigger(StateName.Attack);
            currentTarget = null;
        }

        public void AttackDone()
        {
            role.isAttacking = false;
            role.OnIdle();
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
                    moveAI.AbortAI();
                }
                currentTarget = null;
            }
        }
    }
}