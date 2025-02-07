using System.Collections;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public class MoveAIBase : AIBase, IMoveAI
    {
        public override bool IsAlive => moveTask != null;
        protected Coroutine moveTask { get; set; }
        protected IMoveInfo moveInfo { get; set; }
        public IBoid Host { get; }
        protected Rigidbody rb;

        public MoveAIBase(GameRoleCtrl role) : base(role)
        {
            Host = new Boid(role.transform, role.MoveSpeed);
            rb = role.GetComponent<Rigidbody>();
        }

        public virtual void OnMove(IMoveInfo _moveInfo)
        {
            moveInfo = _moveInfo;
            if (!IsAlive)
            {
                //如果处于移动状态就只需要更新目标点
                //如果不是移动状态就需要切换到移动状态
                if (role.currentState != StateName.Move)
                {
                    role.stateMachine.TryTrigger(StateName.Move);
                }

                moveTask = role.StartCoroutine(Move());
            }
        }

        protected IEnumerator Move()
        {
            while (!moveInfo.IsArrive()) //如果未到达就继续移动
            {
                UpdatePosAndDir();
                yield return null;
            }

            if (moveInfo.OnArrive == null)
            {
                role.OnIdle();
            }
            else
            {
                moveInfo.OnArrive();
            }
            Debug.Log($"{role.name}结束移动");

            moveTask = null;
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