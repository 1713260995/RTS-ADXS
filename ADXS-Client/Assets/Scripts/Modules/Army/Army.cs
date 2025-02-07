using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules.AI;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Assets.Scripts.Modules.SteeringBehaviors;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 部队队
    /// 1.每个Team通过部队控制部队内所有角色
    /// 2.每个Team可以有多个部队
    /// </summary>
    public class Army
    {
        public int id;
        public int agentId;

        private List<GameRoleCtrl> roleCtrlList { get; set; }

        public Army(int agentId)
        {
            id = MyMath.UniqueNum();
            roleCtrlList = new();
            this.agentId = agentId;
        }

        /// <summary>
        /// 替换部队成员
        /// </summary>
        public void ReplaceMember(GameRoleCtrl _newUnits)
        {
            roleCtrlList.Clear();
            if (_newUnits != null) {
                roleCtrlList.Add(_newUnits);
            }
        }


        public void ReplaceMembers(List<GameRoleCtrl> _newUnits)
        {
            if (_newUnits == null) {
                roleCtrlList.Clear();
            }
            else {
                roleCtrlList = _newUnits;
            }
        }

        /// <summary>
        /// 在替换部队员时，获取变化的成员。
        /// 即需要删除的成员id和需要添加的成员id
        /// 供帧同步使用
        /// </summary>
        public void GetMembersByDelta(List<GameRoleCtrl> _newRoles, out List<int> deleteIds, out List<int> addIds)
        {
            //任何在旧集合存在但不在新集合中存在的角色都需要删除
            deleteIds = roleCtrlList.Where(o => _newRoles.Exists(o1 => o1.id == o.id) == false).Select(o => o.id).ToList();

            //任何新集合存在就集合不存在都需要添加
            addIds = _newRoles.Where(o => roleCtrlList.Exists(o1 => o1.id == o.id) == false).Select(o => o.id).ToList();
        }


        #region Command

        public void Move(Vector3 point)
        {
            var list = roleCtrlList.OrderBy(o => (o.transform.position - point).magnitude).ToList();
            IBoid leader = list.First().GetComponent<IBoid>();
            List<Boid> followers = list.Skip(1).Select(o => o.GetComponent<Boid>()).ToList();
            Boid.SetGroup((Boid)leader, followers.Cast<IBoid>().ToList(), 4);
            var info = new MoveInfoByBoid(leader, followers.Cast<IBoid>().ToList(), point, 4, 2, 1.8f);
            roleCtrlList.ForEach(o => { o.OnMove(info); });
        }


        public void Attack(GameUnitCtrl target)
        {
            roleCtrlList.ForEach(o => { o.OnAttack(target); });
        }


        public void Idle()
        {
            roleCtrlList.ForEach(o => { o.OnIdle(); });
        }

        #endregion
    }
}