using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI
{
    public interface IAIBase
    {
        /// <summary>
        /// 是否正在运行该AI
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// 强制中止该AI运行
        /// </summary>
        void AbortAI();
    }
}
