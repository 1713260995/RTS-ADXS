﻿using System;
using System.Collections.Generic;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Modules;
using Assets.Scripts.Modules.AI;
using UnityEngine;

public class GameUnitCtrl : MonoBehaviour
{
    public GameUnitName unitName;
    public GameUnitType unitType;
    public int id { get; private set; }
    public TeamAgent agent { get; private set; }
    public Action OnDead { get; set; }
    public List<IAIBase> aIBases { get; private set; }

    protected virtual void Awake()
    {
        id = MyMath.UniqueNum();
        GameUnitManager.Instance.allGameUnits.Add(this);
    }

    protected virtual void Start()
    { }

    protected virtual void Update()
    { }

    protected virtual void OnDestroy()
    {
        agent?.allUnits.Remove(this);
        GameUnitManager.Instance.allGameUnits.Remove(this);
        OnDead?.Invoke();
    }


    public void SetAgent(TeamAgent _agent)
    {
        agent = _agent;
        agent.allUnits.Add(this);
    }

    protected virtual void InitAI()
    {
        aIBases = new List<IAIBase>();
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

        if (target.agent.groupId != agent.groupId)
        {
            return true;
        }

        return false;
    }
}