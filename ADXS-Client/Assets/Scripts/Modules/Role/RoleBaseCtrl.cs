using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Role;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleBaseCtrl : GameUnitCtrl
{
    public RoleBase entity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init(GameUnit _gameUnit)
    {
        base.Init(gameUnit);
        entity = _gameUnit as RoleBase;
    }
}
