using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enum
{
    public enum GameUnitName
    {
        Unknown = 0,

        #region 环境

        Tree01,

        #endregion

        #region 人族

        /// <summary>
        /// 农民
        /// </summary>
        Peasant,

        /// <summary>
        /// 步兵
        /// </summary>
        Footman,

        /// <summary>
        /// 火枪手
        /// </summary>
        Rifleman,

        /// <summary>
        /// 骑士
        /// </summary>
        Knight,

        /// <summary>
        /// 大法师
        /// </summary>
        Archmage,

        /// <summary>
        /// 山丘之王
        /// </summary>
        MountainKing,

        /// <summary>
        /// 圣骑士
        /// </summary>
        Paladin,

        #endregion
    }
}
