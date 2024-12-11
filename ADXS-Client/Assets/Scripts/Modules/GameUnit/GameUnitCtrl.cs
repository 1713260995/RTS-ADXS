using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using UnityEngine;

public class GameUnitCtrl : MonoBehaviour
{
    public GameUnitName unitName;
    public GameUnitType unitType;

    public int id { get; private set; }
    public Agent agent { get; private set; }

    protected virtual void Awake()
    {
        id = MyMath.UniqueNum();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameUnitManager.Instance.allGameUnits.Add(this);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public virtual void Init(Agent _agent)
    {
        agent = agent;
        agent.gameUnitCtrls.Add(this);
    }


    private void OnDestroy()
    {
        agent?.gameUnitCtrls.Remove(this);
        GameUnitManager.Instance.allGameUnits.Remove(this);
    }


    public bool CanAttack(Agent attacker)
    {
        if (unitType == GameUnitType.GoldMine || unitType == GameUnitType.Tree)
        {
            return false;
        }
        if (attacker.teamId != agent.teamId)
        {
            return true;
        }
        return false;
    }
}
