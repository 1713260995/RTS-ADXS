using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using UnityEngine;

public class GameUnitCtrl : MonoBehaviour
{
    [HideInInspector]
    public int id;
    public GameUnitName unitName;
    public GameUnitType unitType;

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


}
