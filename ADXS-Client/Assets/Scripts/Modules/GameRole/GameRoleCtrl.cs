using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.Role;
using System.Collections.Generic;
using UnityEngine;

public class GameRoleCtrl : GameUnitCtrl
{
    public RoleType roleType;
    public RaceType raceType;

    public RoleState currentState { get; set; }
    public List<BuffBase> buffList { get; private set; }
    public RoleAttributes roleAttributes { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }



}
