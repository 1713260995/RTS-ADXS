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
        Action OnArray { get; }
        /// <summary>
        /// 是否已经到达终点
        /// </summary>
        Func<bool> IsArray { get; }
    }

    public struct MoveInfoByPoint : IMoveInfo
    {
        private Vector3 endPoint;
        private Action onArray;
        private Func<bool> isArray;

        public MoveInfoByPoint(Vector3 endPoint, Func<bool> isArray, Action onArray)
        {
            this.endPoint = endPoint;
            this.onArray = onArray;
            this.isArray = isArray;
        }

        public Vector3 Destination => endPoint;

        public Action OnArray => onArray;

        public Func<bool> IsArray => isArray;
    }

    public struct MoveInfoByObj : IMoveInfo
    {
        private GameObject obj;
        private Action onArray;
        private Func<bool> isArray;

        public MoveInfoByObj(GameObject obj, Func<bool> isArray, Action onArray)
        {
            this.obj = obj;
            this.onArray = onArray;
            this.isArray = isArray;
        }

        public Vector3 Destination => obj.transform.position;

        public Action OnArray => onArray;

        public Func<bool> IsArray => isArray;
    }

}
