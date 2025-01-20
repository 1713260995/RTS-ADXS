using Assets.GameClientLib.Scripts.Utils.FSM;
using Assets.Scripts.Modules.FSM.Role;
using Assets.Scripts.Modules.Role;
using System.Collections.Generic;

namespace Assets.Scripts.Modules
{
    public class FarmerCtrl : RoleByPoolCtrl
    {
        public float buildDistance => attackDistance;

        protected IBuildAI buildAI;

        protected override void InitAI()
        {
            base.InitAI();
            buildAI = new BuildAI(this, moveAI, idleAI);
        }

        protected override List<State> InitRoleStates()
        {
            List<State> list = base.InitRoleStates();
            list.Add(new BuildState());
            return list;
        }

        public void OnLumbering(TreeCtrl tree)
        {
            print("伐木");
        }

        public void OnMining(GoldMine goldMine)
        {
            print("采矿");
        }

        public void OnBuild(BuildInfo info)
        {
            buildAI.OnBuild(info);
        }
    }
}
