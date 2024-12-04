using Assets.Scripts.Modules.Command;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class Army
    {
        private List<GameRoleCtrl> roleCtrls { get; set; }

        public Army()
        {
            roleCtrls = new List<GameRoleCtrl>();
        }

        /// <summary>
        /// 替换部队成员
        /// </summary>
        /// <param name="_roles"></param>
        public void ReplaceMembers(List<GameRoleCtrl> _roles)
        {
            roleCtrls = _roles;
        }

        /// <summary>
        /// 增量更新部队成员
        /// </summary>
        /// <param name="_roles"></param>
        public void UpdateMembersByDelta(List<GameRoleCtrl> _roles)
        {
            //任何不在新集合中存在的角色需要删除
            List<int> deleteId = roleCtrls.Where(o => _roles.Any(o1 => o1.id == o.id) == false).Select(o => o.id).ToList();
        }

        public void Move(Vector3 targetPoint)
        {
            roleCtrls.ForEach(o =>
            {
                if (o is IMove move)
                {
                    move.Move(targetPoint);
                }
            });
        }

        public void Attack(List<GameRoleCtrl> roleList)
        {
            if (roleList.Count == 1)
            {
                roleCtrls.ForEach(o =>
                {
                    if (o is IAttack a) a.Attack(roleList[0]);
                });
            }
            else
            {
                throw new System.NotImplementedException("未完成攻击群体目标");
            }
        }

        public void Idle()
        {
            roleCtrls.ForEach(o =>
            {
                if (o is IIdle a) a.Idle();
            });
        }
    }
}
