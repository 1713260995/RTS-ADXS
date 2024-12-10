using Assets.GameClientLib.Scripts;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Battle;
using Assets.Scripts.Modules.Role;
using Assets.Scripts.Modules.Spawn;
using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Test : MonoBehaviour
{




    [ShowButton]
    public void TestAgent()
    {
        Agent agent1 = new Agent(1, GameColor.Red, new KeyboardCommand());
        BattleSystem.Instance.AddAgent(agent1);
        BattleSystem.Instance.CreateGameUnit<GameUnitCtrl>(GameUnitName.Peasant, agent1.id, Vector3.zero);
    }


}
