using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Assets.Scripts.Modules.Spawn;
using Assets.Scripts.Modules.Role;

namespace Assets.Scripts.Test.UnitTest
{

    /// <summary>
    /// 创建一个农民，控制农民建造建筑物
    /// </summary>
    public class UnitTest_Build : UnitTest_Base
    {
        public GameUnitName buildingName = GameUnitName.Barracks;

        private FarmerCtrl peasant;
        
        [ShowButton]
        public void CreateFarmer()
        {
            TeamAgent agent = new TeamAgent(1, GameColor.Red, AgentControlWay.Keyboard);
            BattleSystem.Instance.AddAgent(agent);
            peasant = BattleSystem.Instance.CreateUnit<FarmerCtrl>(GameUnitName.Peasant, Vector3.zero, agent.id);
            BattleSystem.Instance.StartGame();
        }


        [ShowButton]
        public void TestBuild()
        {
            if (peasant == null)
            {
                CreateFarmer();
            }
            BuildSystem.Instance.EnterPreview(buildingName, peasant);
        }




        protected override void CheckRequireModule()
        {
            if (BuildSystem.Instance == null)
            {
                throw new ArgumentNullException();
            }
            if (SpawnSystem.Instance == null)
            {
                throw new ArgumentNullException();
            }
            if (BattleSystem.Instance == null)
            {
                throw new ArgumentNullException();
            }
        }

    }
}
