using System.Collections;
using Assets.GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIBase : AIBase, IMoveAI
    {
        public override bool IsAlive => moveTask != null;
        protected Coroutine moveTask { get; set; }
        protected MoveInfo moveInfo { get; set; }
        public MoveAIBase(GameRoleCtrl role) : base(role)
        {
        }

        public virtual void OnMove(MoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            if (!IsAlive)
            {
                if (role.currentState != StateName.Move)
                {
                    role.stateMachine.TryTrigger(StateName.Move);
                }
                moveTask = role.StartCoroutine(Move());
            }
        }

        protected IEnumerator Move()
        {
            while ((moveInfo.endPoint - transform.position).magnitude > moveInfo.moveStopDis)
            {
                UpdatePosAndDir();
                yield return null;
            }

            moveInfo.onComplete?.Invoke();
            Debug.Log("到达终点");
        }
        /// <summary>
        /// 更新移动时位置和方向
        /// </summary>
        protected virtual void UpdatePosAndDir()
        {
            Vector3 rotateEuler = MyMath.LookAt(transform, moveInfo.endPoint);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateEuler, MyMath.GetLerp(role.rotateLerp));
            Vector3 currentVelocity = (moveInfo.endPoint - transform.position).normalized * (role.MoveSpeed * Time.deltaTime);
            transform.position += currentVelocity;
        }

        public override void AbortAI()
        {
            if (IsAlive)
            {
                role.StopCoroutine(moveTask);
                moveTask = null;
            }
        }
    }
}