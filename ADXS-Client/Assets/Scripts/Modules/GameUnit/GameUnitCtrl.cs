using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules;
using UnityEngine;

public class GameUnitCtrl : MonoBehaviour
{

    public int id;
    public GameUnit unitEnity;

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

    public virtual void Init(GameUnit _gameUnit)
    {
        unitEnity = _gameUnit;
    }
}
