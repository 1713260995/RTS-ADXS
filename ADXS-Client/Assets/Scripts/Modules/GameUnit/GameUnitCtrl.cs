using Assets.GameClientLib.Scripts.Utils;
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
        GameUnitManager.Instance.allGameUnits.Add(this);
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        agent?.allUnits.Remove(this);
        GameUnitManager.Instance.allGameUnits.Remove(this);
    }




    public void SetAgent(Agent _agent)
    {
        agent = _agent;
        agent.allUnits.Add(this);
    }


    /// <summary>
    /// 判断目标能否被我攻击
    /// </summary>
    public virtual bool CanAttack(GameUnitCtrl target)
    {
        if (target.unitType == GameUnitType.GoldMine || target.unitType == GameUnitType.Tree)
        {
            return false;
        }
        if (target.agent.teamId != agent.teamId)
        {
            return true;
        }
        return false;
    }
}
