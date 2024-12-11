using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 控制整个团队
    /// </summary>
    public interface IAgentControl
    {
        void OpenControl();

        void CloseControl();
    }
}
