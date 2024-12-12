using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enum
{
    public enum RoleAnimName
    {
        Unknown,

        Idle,
        Walk,
        Attack,
        Death,

        #region 农民
        Gold,
        Tree,
        AttackGold,
        AttackTree,
        #endregion

        #region 技能
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        #endregion
    }
}
