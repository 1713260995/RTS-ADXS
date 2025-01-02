using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using UnityEngine;

namespace Test
{
    /// <summary>
    /// 测试接入ORCA后避障效果
    /// 
    /// </summary>
    public class UnitTest_ORCA
    {
        public GameColor groupPlayerColor = GameColor.Blue;

        // [ShowButton]
        // public void Test() {
        //     Agent agent1 = new Agent(1, groupAIColor, AgentControlWay.Keyboard);
        //
        //     BattleSystem.Instance.AddAgent(agent1);
        //     BattleSystem.Instance.AddAgent(agent2);
        //     BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent1.id, birthLocation);
        //     BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent2.id, new Vector3(5, 0, 0));
        //     BattleSystem.Instance.StartGame();
        // }
        //
        // [Header("GroupAI")]
        // public GameColor groupAIColor = GameColor.Red;
        // //  public 
        //
        //
        // private void CreateGroupAI() {
        //     Agent agent2 = new Agent(2, groupPlayerColor, AgentControlWay.AI);
        //     BattleSystem.Instance.AddAgent(agent2);
        //
        //     BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent1.id, birthLocation);
        // }
    }
}