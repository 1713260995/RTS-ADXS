using Assets.GameClientLib.Scripts.Utils.Factory;
using Assets.Scripts.Common.Enum;
using UnityEngine;

namespace Assets.Scripts.Modules.Role
{
    public abstract class GameUnitFactorySO<T> : FactorySO<T> where T : GameUnit
    {
        public GameObject prefab;
        public T role { get; set; }
        public GameUnitType unitType { get; }
    }
}
