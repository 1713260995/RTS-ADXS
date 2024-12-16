using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Common.Enum
{
    public static class EnumHelper
    {
        public static int GetAnimHash(this RoleAnimName name)
        {
            return Animator.StringToHash(name.ToString());
        }

        /// <summary>
        /// 1-32
        /// </summary>
        public static int GetLayer(this GameLayerName gameLayer)
        {
            return LayerMask.NameToLayer(gameLayer.ToString());
        }

        /// <summary>
        /// 1-32转为二进制
        /// </summary>
        public static int GetLayerMask(this GameLayerName[] gameLayers)
        {
            var s = gameLayers.Select(o => o.ToString()).ToArray();
            return LayerMask.GetMask(s);
        }

        /// <summary>
        /// 1-32转为二进制
        /// </summary>
        public static int GetLayerMask(this GameLayerName gameLayer)
        {
            return LayerMask.GetMask(gameLayer.ToString());
        }
    }
}
