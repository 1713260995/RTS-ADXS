using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class NoramlAttackAI : IAttackAI
    {
        protected GameRoleCtrl role { get; private set; }

        /// <summary>
        /// 是否正在处理攻击任务
        /// </summary>
        public bool isProcessing { get; private set; }


        public NoramlAttackAI(GameRoleCtrl role)
        {
            this.role = role;
            isProcessing = false;
        }

        public void OnAttack(GameUnitCtrl target)
        {
            if (!role.CanAttack(target))
                return;
            AbortPrevAttack();
            isProcessing = true;
            role.moveAI.OnFollow(target, role.attackDistance, () => { Attack(target); });//追踪目标直至到达攻击距离
        }

        protected void Attack(GameUnitCtrl target)
        {
            role.stateMachine.TryTrigger(StateName.Attack);
            isProcessing = false;
        }


        public void AbortPrevAttack(StateName? nextState = null)
        {
            if (isProcessing)
            {
                if (role.currentState == StateName.Move)
                {//如果当前运行攻击AI，并在在追踪目标。则结束此任务
                    role.moveAI.AbortTask();
                }
                if (nextState != null)
                {
                    role.stateMachine.TryTrigger(nextState.Value);
                }
            }
        }
    }
}