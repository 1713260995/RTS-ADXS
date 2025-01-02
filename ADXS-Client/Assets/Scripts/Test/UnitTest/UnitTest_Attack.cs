using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using UnityEngine;

namespace Test
{
    public class UnitTest_Attack : MonoBehaviour
    {
        public GameColor color = GameColor.Red;
        public GameUnitName unitName = GameUnitName.Peasant;
        public Vector3 birthLocation = Vector3.zero;

        /// <summary>
        /// 测试，
        /// </summary>
        [ShowButton]
        public void TestAttack() {
            TeamAgent agent1 = new TeamAgent(1, color, AgentControlWay.Keyboard);
            TeamAgent agent2 = new TeamAgent(2, color, AgentControlWay.AI);
            BattleSystem.Instance.AddAgent(agent1);
            BattleSystem.Instance.AddAgent(agent2);
            BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent1.id, birthLocation);
            BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent2.id, new Vector3(5, 0, 0));
            BattleSystem.Instance.StartGame();
        }
    }
}