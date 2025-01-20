using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public interface IMoveAI : IAIBase
    {
        /// <summary>
        /// 到达指定位置
        /// </summary>
        void OnMove(IMoveInfo info);
    }
}
