using System;
using UnityEngine;


namespace Assets.Scripts.Modules.AI
{
    public interface IFindWay
    {
        /// <summary>
        /// 开始寻路，如果路径合法，返回true
        /// </summary>
        bool FindWay(Vector3 point, Action onComplete);
    }
}
