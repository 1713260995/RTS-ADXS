using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 控制整个团队
    /// </summary>
    public interface IAgentControl
    {
        Agent agent { get; set; }

        void OpenControl();

        void CloseControl();
    }
}
