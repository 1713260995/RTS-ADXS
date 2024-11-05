namespace Assets.Scripts.Modules
{
    public class RoleAttributes : GameUnitAttributes
    {
        public float currentHP { get; set; }
        public float maxHP { get; set; }
        public float HPRestore { get; set; }//生命恢复
        public float MP { get; set; }
        public float maxMP { get; set; }
        public float MPRestore { get; set; }//魔法恢复
        public float defense { get; set; }
        public float attack { get; set; }
        public float attackSpeed { get; set; }
        public float moveSpeed { get; set; }

        public RoleAttributes()
        {
            isInvincible = false;
        }
    }
}
