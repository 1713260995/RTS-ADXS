using Assets.Scripts.Modules.SteeringBehaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public interface IMoveAI : IAIBase
    {
        IBoid Host { get; }


        /// <summary>
        /// 到达指定位置
        /// </summary>
        void OnMove(IMoveInfo info);

    }
}
