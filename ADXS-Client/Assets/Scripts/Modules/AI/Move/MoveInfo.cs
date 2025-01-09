using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules.AI
{
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
