using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DynamicUnit : GameUnit
{

    public IControlUnit control;

    protected DynamicUnit(IControlUnit _control)
    {
        control = _control;
    }

    public abstract void Move();
    public abstract void Attack();


}


public interface IControlUnit
{
    void Control();
}