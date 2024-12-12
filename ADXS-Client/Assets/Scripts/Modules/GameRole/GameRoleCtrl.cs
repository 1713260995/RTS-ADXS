using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.Buff;
using Assets.Scripts.Modules.FSM;
using Assets.Scripts.Modules.Role;
using System.Collections.Generic;
using UnityEngine;

public class GameRoleCtrl : GameUnitCtrl, IIdle, IAttack, IMove
{
    public RoleType roleType;
    public RaceType raceType;

    public RoleState currentState { get; set; }
    public List<BuffBase> buffList { get; private set; }
    public RoleAttributes roleAttributes { get; private set; }
    public Animator animator { get; private set; }

    protected int walkHash { get; set; }
    protected int attackHash { get; set; }
    protected int idleHash { get; set; }


    private void Start()
    {
        animator = GetComponent<Animator>();
        walkHash = RoleAnimName.Walk.GetAnimHash();
    }


    public virtual void Attack(GameUnitCtrl role)
    {
        Debug.Log("开始攻击");

    }

    public void Idle()
    {
        animator.SetBool(idleHash, true);
        Debug.Log("休息");
    }

    public void Move(Vector3 targetPoint)
    {
        Debug.Log("移动");
    }

}
