using Assets.Scripts.Modules;
using UnityEngine;

public class GameUnitCtrl : MonoBehaviour
{
    public GameUnit unitEnity;



    // Start is called before the first frame update
    void Start()
    {
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
