using Assets.GameClientLib.Scripts;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Role;
using Assets.Scripts.Modules.Spawn;
using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Test : MonoBehaviour
{
    [Header("AddAgent")]
    public int teamId = 1;
    public GameColor color = GameColor.Red;
    public AgentControlWay controlWay = AgentControlWay.Keyboard;

    [ShowButton]
    public void AddAgent()
    {
        Agent agent1 = new Agent(teamId, color, controlWay);
        BattleSystem.Instance.AddAgent(agent1);
        agent1.OnEnable();
        Debug.Log("AddAgent:" + agent1);
    }

    [Header("AddGameUnit")]
    public int agentId = 1;
    public GameUnitName unitName = GameUnitName.Peasant;
    public Vector3 unitPos = Vector3.zero;

    [ShowButton]
    public void AddGameUnit()
    {
        BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agentId, unitPos);
    }

    [ShowButton]
    public void PrintAllAgents()
    {
        BattleSystem.Instance.agents.ForEach(o => Debug.Log(o));
    }

    [ShowButton]
    public void TestAttack()
    {
        Agent agent1 = new Agent(1, color, AgentControlWay.Keyboard);
        Agent agent2 = new Agent(2, color, AgentControlWay.AI);
        BattleSystem.Instance.AddAgent(agent1);
        BattleSystem.Instance.AddAgent(agent2);
        BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent1.id, unitPos);
        BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(unitName, agent2.id, new Vector3(5, 0, 0));

        BattleSystem.Instance.StartGame();
    }



}
