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

        public static bool IsArriveByRaycast(GameUnitCtrl origin, GameUnitCtrl target, float dis)
        {
            Transform t = origin.transform;
            if (Physics.Raycast(t.position, t.forward, out RaycastHit hitInfo, dis, GameLayerName.GameUnit.GetLayerMask()))
            {
                if (hitInfo.collider.gameObject.GetInstanceID() == target.gameObject.GetInstanceID())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
