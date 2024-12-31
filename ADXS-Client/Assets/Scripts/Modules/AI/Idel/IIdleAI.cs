using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Modules.AI.Idel
{
    public interface IIdleAI : IAIBase
    {
        void OnIdle();
    }
}
