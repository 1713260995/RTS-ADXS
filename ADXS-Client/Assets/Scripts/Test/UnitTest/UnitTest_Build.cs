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

namespace Assets.Scripts.Test.UnitTest
{

    /// <summary>
    /// 创建一个农民，控制农民建造建筑物
    /// </summary>
    public class UnitTest_Build : MonoBehaviour
    {


        [ShowButton]
        public void TestBuild()
        {
            TeamAgent agent1 = new TeamAgent(1, GameColor.Red, AgentControlWay.Keyboard);
            GameRoleCtrl Peasant = SpawnSystem.Instance.CreateCtrl<GameRoleCtrl>(GameUnitName.Peasant);

        }

    }
}
