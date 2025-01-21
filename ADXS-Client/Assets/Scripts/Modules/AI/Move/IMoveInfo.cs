using System;
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
        private Vector3 endPoint;
        private Action onArrive;
        private Func<bool> isArrive;
        public Vector3 Destination => endPoint;
        public Action OnArrive => onArrive;
        public Func<bool> IsArrive => isArrive;


        public MoveInfoByPoint(Vector3 endPoint, Func<bool> isArrive, Action onArrive)
        {
            this.endPoint = endPoint;
            this.onArrive = onArrive;
            this.isArrive = isArrive;
        }

    }

    public struct MoveInfoByObj : IMoveInfo
    {
        private GameObject obj;
        private Action onArrive;
        private Func<bool> isArrive;
        public Vector3 Destination => obj.transform.position;

        public Action OnArrive => onArrive;

        public Func<bool> IsArrive => isArrive;


        public MoveInfoByObj(GameObject obj, Func<bool> isArrive, Action onArrive)
        {
            this.obj = obj;
            this.onArrive = onArrive;
            this.isArrive = isArrive;
        }

    }

}
