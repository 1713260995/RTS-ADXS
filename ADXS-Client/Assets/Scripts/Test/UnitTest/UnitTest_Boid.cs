using System;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Spawn;
using Assets.Scripts.Test.UnitTest;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitTest_Boid : UnitTest_Base
{
    public GameColor color = GameColor.Red;
    public GameUnitName unitName = GameUnitName.Peasant;
    public float unitNum = 10;
    public float unitCreateRadius = 5;

    /// <summary>
    /// 测试
    /// </summary>
    [ShowButton]
    public void TestBoid()
    {
        TeamAgent agent1 = new TeamAgent(1, color, AgentControlWay.Keyboard);
        BattleSystem.Instance.AddAgent(agent1);
        BattleSystem.Instance.StartGame();
        for (int i = 0; i < unitNum; i++)
        {
            Vector2 birthLocation = Random.insideUnitCircle * unitCreateRadius;
            var unit = BattleSystem.Instance.CreateUnit<GameUnitCtrl>(unitName, birthLocation.XZToXYZ(), agent1.id);
            unit.name = unitName.ToString() + i;
        }
    }


    protected override void CheckRequireModule()
    {
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