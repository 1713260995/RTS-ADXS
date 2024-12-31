using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public interface IFollowAI : IAIBase
    {
        /// <summary>
        /// 跟随目标直至距离差小于stopDis
        /// </summary>
        void OnFollow(FollowInfo followInfo);
    }


    public struct FollowInfo
    {
        public GameUnitCtrl target;
        public float stopDis;
        public Action onComplete;
        public Vector3 currentTargetPos => target.transform.position;

        public FollowInfo(GameUnitCtrl target, float stopDis, Action onComplete)
        {
            this.target = target;
            this.stopDis = stopDis;
            this.onComplete = onComplete;
        }
    }
}
