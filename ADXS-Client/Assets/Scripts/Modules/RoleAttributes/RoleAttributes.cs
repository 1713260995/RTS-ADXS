using System;

namespace Assets.Scripts.Modules
{
    [Serializable]
    public class RoleAttributes : UnitAttributes
    {
        public float currentHP;
        public float maxHP;
        public float HPRestore;//生命恢复
        public float currentMP;
        public float maxMP;
        public float MPRestore;//魔法恢复
        public float defense;
        public float attack;
        public float attackSpeed;
        public float moveSpeed;

        public RoleAttributes()
        {
            isInvincible = false;
        }
    }
}
