using Assets.Scripts.Modules.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class ArriveWay
    {

        public static bool IsArriveByDistance(Vector3 origin, Vector3 point, float distance)
        {
            bool result = (origin - point).magnitude <= distance;
            return result;
        }

        public static bool IsArriveByDistance(GameRoleCtrl o, Vector3 point)
        {
            return IsArriveByDistance(o.transform.position, point, o.moveStopDis);
        }

        public static bool IsArriveByAttackDistance(GameRoleCtrl origin, GameUnitCtrl target)
        {
            return IsArriveByDistance(origin.transform.position, target.transform.position, origin.attackDistance);
        }

        /// <summary>
        /// 是否到达终点。
        /// 解决两者距离过近但是中间有障碍物的问题。
        /// </summary>
        public static bool IsArriveByRaycast(GameUnitCtrl origin, GameUnitCtrl target, float distance)
        {
            Transform t = origin.transform;
            Physics.Raycast(t.position, t.forward, out RaycastHit hitInfo, distance, GameLayerName.GameUnit.GetLayerMask());
            return hitInfo.collider.gameObject.GetInstanceID() == target.gameObject.GetInstanceID();
        }
    }
}
