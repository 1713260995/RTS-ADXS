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

    public virtual void Init(Agent _agent)
    {
        agent = _agent;
        agent.allUnits.Add(this);
    }


    private void OnDestroy()
    {
        agent?.allUnits.Remove(this);
        GameUnitManager.Instance.allGameUnits.Remove(this);
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
