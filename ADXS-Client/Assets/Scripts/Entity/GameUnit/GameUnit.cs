using Assets.Scripts.Common.Enum;
using System;

public class GameUnit
{
    public string id;
    public GameUnitProperty property;
    public GameUnitType unitType;

    public GameUnit()
    {
        id = Guid.NewGuid().ToString();
    }
}
