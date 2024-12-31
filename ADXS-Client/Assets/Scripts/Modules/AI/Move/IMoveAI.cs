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
        void OnMove(MoveInfo info);
    }

    public struct MoveInfo
    {
        public Vector3 endPoint;
        public float moveStopDis;
        public Action onComplete;

        public MoveInfo(Vector3 endPoint, float moveStopDis, Action onComplete)
        {
            this.endPoint = endPoint;
            this.moveStopDis = moveStopDis;
            this.onComplete = onComplete;
        }
    }
}
