using Assets.Scripts.Modules.GameRole;

namespace Assets.Scripts.Modules
{
    public class FarmerCtrl : RoleByPoolCtrl, ILumbering, IMining
    {
        public void Lumbering(TreeCtrl tree)
        {
            print("伐木");
        }

        public void Mining(GoldMine goldMine)
        {
            print("采矿");
        }
    }
}
