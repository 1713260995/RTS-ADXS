using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Modules.AI.Follow
{
    public class FollowAI : AIBase, IFollowAI
    {
        protected Coroutine followTask { get; set; }
        FollowInfo followInfo { get; set; }
        public override bool IsAlive => followTask != null;

        public FollowAI(GameRoleCtrl role) : base(role)
        {

        }


        public void OnFollow(FollowInfo followInfo)
        {
            this.followInfo = followInfo;
            if (!IsAlive)
            {
                followTask = role.StartCoroutine(Follow());
            }
        }

        protected IEnumerator Follow()
        {
            while ((role.transform.position - followInfo.currentTargetPos).magnitude > followInfo.stopDis)
            {
                MoveInfo moveInfo = new MoveInfo(followInfo.currentTargetPos, followInfo.stopDis, null);
                role.moveAI.OnMove(moveInfo);
                yield return null;
            }
            Debug.Log("跟随结束");
            role.moveAI.AbortAI();
            followInfo.onComplete?.Invoke();
            followTask = null;
        }

        //moveAI.AbortAI()内部已转换到idle
        public override void AbortAI()
        {
            if (IsAlive)
            {
                role.StopCoroutine(followTask);
                role.moveAI.AbortAI();
                followTask = null;
            }
        }
    }
}
