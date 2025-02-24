using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedTransformList : SharedVariable<List<Transform>>
    {
        public SharedTransformList()
        {
            mValue = new List<Transform>();
        }

        public static implicit operator SharedTransformList(List<Transform> value) { return new SharedTransformList { mValue = value }; }
    }
}