using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI
{
    public interface IAttackAI
    {
        /// <summary>
        /// 执行攻击命令
        /// </summary>
        void OnAttack(GameUnitCtrl target);

        /// <summary>
        /// 结束上一个攻击任务
        /// </summary>
        void AbortPrevAttack(StateName? nextState = null);
    }



}
