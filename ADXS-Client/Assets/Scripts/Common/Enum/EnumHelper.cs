using UnityEngine;

namespace Assets.Scripts.Common.Enum
{
    public static class EnumHelper
    {
        public static int GetAnimHash(this RoleAnimName name)
        {
            return Animator.StringToHash(name.ToString());
        }
    }
}
