using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Test.UnitTest;
using UnityEngine;

namespace Test
{
    /// <summary>
    /// 测试接入ORCA后避障效果
    /// </summary>
    public class UnitTest_ORCA : UnitTest_Base
    {
        public float interval = 1.5f;
        public GameUnitName unitName = GameUnitName.Peasant;

        [ShowButton]
        public void Test()
        {
            CreateGroupAI();
            CreateGroupPlayer();
            BattleSystem.Instance.StartGame();
        }

        private void CreateGroupAI()
        {
            TeamAgent agent = new TeamAgent(2, GameColor.Red, AgentControlWay.AI);
            BattleSystem.Instance.AddAgent(agent);
            Vector3 birthPos = new Vector3(7, 0, 7);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vector3 pos = new Vector3(birthPos.x + interval * i, 0, birthPos.z + interval * j);
                    BattleSystem.Instance.CreateUnit<GameUnitCtrl>(unitName, pos, agent.id);
                }
            }
        }


        private void CreateGroupPlayer()
        {
            TeamAgent agent = new TeamAgent(2, GameColor.Red, AgentControlWay.Keyboard);
            BattleSystem.Instance.AddAgent(agent);
            Vector3 birthPos = new Vector3(-7, 0, -7);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Vector3 pos = new Vector3(birthPos.x - interval * i, 0, birthPos.z - interval * j);
                    BattleSystem.Instance.CreateUnit<GameUnitCtrl>(unitName, pos, agent.id);
                }
            }
        }
    }
}