using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI
{
    public interface IAttackAI : IAIBase
    {
        /// <summary>
        /// 执行攻击命令
        /// 如果正在追击目标，则切换到新的攻击目标
        /// </summary>
        void OnAttack(GameUnitCtrl target);


        void AttackDone();

    }



}
