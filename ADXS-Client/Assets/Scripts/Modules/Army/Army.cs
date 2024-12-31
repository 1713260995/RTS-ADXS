using Assets.GameClientLib.Scripts.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Modules
{
    public class Army
    {
        public int id;
        public int agentId;

        private List<GameRoleCtrl> roleCtrls { get; set; }

        public Army(int agentId)
        {
            id = MyMath.UniqueNum();
            roleCtrls = new();
            this.agentId = agentId;
        }

        /// <summary>
        /// 替换部队成员
        /// </summary>
        public void ReplaceMember(GameRoleCtrl _newUnits)
        {
            roleCtrls.Clear();
            if (_newUnits != null)
            {
                roleCtrls.Add(_newUnits);
            }
        }


        public void ReplaceMembers(List<GameRoleCtrl> _newUnits)
        {
            if (_newUnits == null)
            {
                roleCtrls.Clear();
            }
            else
            {
                roleCtrls = _newUnits;
            }
        }

        /// <summary>
        /// 在替换部队员时，获取变化的成员。
        /// 即需要删除的成员id和需要添加的成员id
        /// 供帧同步使用
        /// </summary>
        /// <param name="_roles"></param>
        public void GetMembersByDelta(List<GameRoleCtrl> _newRoles, out List<int> deleteIds, out List<int> addIds)
        {
            //任何在旧集合存在但不在新集合中存在的角色都需要删除
            deleteIds = roleCtrls.Where(o => _newRoles.Exists(o1 => o1.id == o.id) == false).Select(o => o.id).ToList();

            //任何新集合存在就集合不存在都需要添加
            addIds = _newRoles.Where(o => roleCtrls.Exists(o1 => o1.id == o.id) == false).Select(o => o.id).ToList();
        }


        #region Command

        public void Move(Vector3 point)
        {
            roleCtrls.ForEach(o =>
            {
                o.moveAI.OnMove(point);
            });
        }

        public void Attack(GameUnitCtrl target)
        {
            roleCtrls.ForEach(o =>
            {
                o.attackAI.OnAttack(target);
            });
        }


        public void Idle()
        {
            roleCtrls.ForEach(o =>
            {
                o.idleAI.OnIdle();
            });
        }



        #endregion
    }
}
