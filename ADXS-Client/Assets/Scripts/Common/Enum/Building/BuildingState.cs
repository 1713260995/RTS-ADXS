using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Common.Enum
{
    public enum BuildingState
    {
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknown,
        /// <summary>
        /// 预览正常
        /// </summary>
        NormalPreview,
        /// <summary>
        /// 预览失败
        /// </summary>
        FailedPreview,
        /// <summary>
        /// 正在建造
        /// </summary>
        UnderConstruction,
        /// <summary>
        /// 建造完成
        /// </summary>
        Completed,
        /// <summary>
        /// 修复中
        /// </summary>
        Repairing
    }
}
