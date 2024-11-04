using Assets.Scripts.Common.Enum;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    [Serializable]
    public abstract class GameUnit
    {
        public int unitId { get; set; }
        public string unitName { get; set; }
        public GameUnitType unitType { get; set; }
    }
}