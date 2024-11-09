using Assets.Scripts.Common.Enum;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    [Serializable]
    public class GameUnit
    {
        public GameUnitName unitName;
        public GameUnitType unitType;
    }
}