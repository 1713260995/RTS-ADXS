using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Move
{
    public interface IMoveAI
    {
        /// <summary>
        /// 到达指定位置
        /// </summary>
        void OnMove(Vector3 endPoint, Action onComplete = null);

        /// <summary>
        /// 跟随目标直至距离差小于stopDis
        /// </summary>
        void OnFollow(GameUnitCtrl target, float stopDis, Action onComplete = null);

        /// <summary>
        /// 中止（移动/跟随）
        /// </summary>
        void AbortTask();
    }
}
