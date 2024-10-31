using Assets.Scripts.Common.Enum;
using System;
using UnityEngine;

namespace Assets.Scripts.Modules
{

    public class GameUnit : MonoBehaviour
    {
        public string id { get; set; }
        public GameUnitType unitType { get; set; }

        public GameUnit()
        {
            id = Guid.NewGuid().ToString();
        }



    }
}