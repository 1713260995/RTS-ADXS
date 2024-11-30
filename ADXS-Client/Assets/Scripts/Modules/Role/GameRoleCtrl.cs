using Assets.Scripts.Modules;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.Role;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using UnityEngine;

public class GameRoleCtrl : GameUnitCtrl, CanExecuteCommands
{
    public GameRole roleEntity;
    public Animator animator { get; private set; }
    public RoleStateMachine stateMachine => roleEntity.stateMachine;


    public List<CommandId> CommandIds => roleEntity.commandIds;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        roleEntity.stateMachine.Restart();
    }

    // Update is called once per frame
    void Update()
    {
        roleEntity.stateMachine.OnUpdate();
    }

    public override void Init(GameUnit _gameUnit)
    {
        base.Init(_gameUnit);
        roleEntity = _gameUnit as GameRole;

    }
}
