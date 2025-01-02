using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using UnityEngine;

public class TestNormal : MonoBehaviour
{
    [Header("AddAgent")]
    public int teamId = 1;
    public GameColor color = GameColor.Red;
    public AgentControlWay controlWay = AgentControlWay.Keyboard;

    [ShowButton]
    public void AddAgent()
    {
        TeamAgent agent1 = new TeamAgent(teamId, color, controlWay);
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
    public void StopBattle() {
        BattleSystem.Instance.StopGame();
    }




}
