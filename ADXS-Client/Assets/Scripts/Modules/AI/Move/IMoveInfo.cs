using System;
using System.Collections.Generic;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
    public interface IMoveInfo
    {
        /// <summary>
        /// 终点
        /// </summary>
        Vector3 Destination { get; }

        /// <summary>
        /// 到达终点回调
        /// </summary>
        Action OnArrive { get; }

        /// <summary>
        /// 是否已经到达终点
        /// </summary>
        Func<bool> IsArrive { get; }
    }

    public struct MoveInfoByPoint : IMoveInfo
    {
        public Vector3 Destination { get; }

        public Action OnArrive { get; }

        public Func<bool> IsArrive { get; }


        public MoveInfoByPoint(Vector3 endPoint, Func<bool> isArrive, Action onArrive)
        {
            this.Destination = endPoint;
            this.OnArrive = onArrive;
            this.IsArrive = isArrive;
        }
    }

    public struct MoveInfoByObj : IMoveInfo
    {
        private GameObject target;
        public Vector3 Destination => target.transform.position;
        public Action OnArrive { get; }
        public Func<bool> IsArrive { get; }


        public MoveInfoByObj(GameObject target, Func<bool> isArrive, Action onArrive)
        {
            this.target = target;
            this.OnArrive = onArrive;
            this.IsArrive = isArrive;
        }
    }

    public struct MoveInfoByBoid : IMoveInfo
    {
        public SteeringBehavior.BoidBehaviorInfo info;
        public Vector3 Destination { get; }
        public Action OnArrive { get; }
        public Func<bool> IsArrive { get; set; }

        public MoveInfoByBoid(List<IBoid> boids, Vector3 destination, int groupNum, float followDistance, float SeparateDistance) : this()
        {
            info = new SteeringBehavior.BoidBehaviorInfo(boids, destination.GetXZ()) {
                groupNum = groupNum,
                separateDistance = SeparateDistance
            };
            this.Destination = destination;
            this.OnArrive = null;
            this.IsArrive = IsArriveFunc;
        }

        private bool IsArriveFunc()
        {
            bool result = true;
            foreach (var item in info.boids) {
                if (item.Velocity.magnitude > 0.01f) {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public void CreateGroup()
        {
            info.CreateGroup();
        }
    }
}