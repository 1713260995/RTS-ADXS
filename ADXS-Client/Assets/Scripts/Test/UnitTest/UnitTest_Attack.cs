using System;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Test.UnitTest;
using UnityEngine;

namespace Test
{
    public class UnitTest_Attack : UnitTest_Base
    {
        public GameColor color = GameColor.Red;
        public GameUnitName unitName = GameUnitName.Peasant;
        public Vector3 birthLocation = Vector3.zero;
        

        /// <summary>
        /// 测试，
        /// </summary>
        [ShowButton]
        public void TestAttack()
        {
            TeamAgent agent1 = new TeamAgent(1, color, AgentControlWay.Keyboard);
            TeamAgent agent2 = new TeamAgent(2, color, AgentControlWay.AI);
            BattleSystem.Instance.AddAgent(agent1);
            BattleSystem.Instance.AddAgent(agent2);
            BattleSystem.Instance.StartGame();
            BattleSystem.Instance.CreateUnit<GameUnitCtrl>(unitName, birthLocation, agent1.id);
            BattleSystem.Instance.CreateUnit<GameUnitCtrl>(unitName, new Vector3(5, 0, 0), agent2.id);

        }
    }
}