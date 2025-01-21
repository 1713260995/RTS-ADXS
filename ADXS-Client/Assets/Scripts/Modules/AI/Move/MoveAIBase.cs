using System.Collections;
using Assets.GameClientLib.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIBase : AIBase, IMoveAI
    {
        public override bool IsAlive => moveTask != null;
        protected Coroutine moveTask { get; set; }
        protected IMoveInfo moveInfo { get; set; }

        public MoveAIBase(GameRoleCtrl role) : base(role) { }

        public virtual void OnMove(IMoveInfo _moveInfo)
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
            while (!moveInfo.IsArrive())//如果未到达就继续移动
            {
                UpdatePosAndDir();
                yield return null;
            }

            moveInfo.OnArrive?.Invoke();
            moveTask = null;
            // Debug.Log("到达终点");
        }
        /// <summary>
        /// 更新移动时位置和方向
        /// </summary>
        protected virtual void UpdatePosAndDir()
        {
            Vector3 rotateEuler = MyMath.LookAt(transform, moveInfo.Destination);
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotateEuler, MyMath.GetLerp(role.rotateLerp));
            Vector3 currentVelocity = (moveInfo.Destination - transform.position).normalized * (role.MoveSpeed * Time.deltaTime);
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