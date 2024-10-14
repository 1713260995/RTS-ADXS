using Assets.Scripts.Modules.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scene
{
    public class BattleSceneCtrl : MonoBehaviour
    {
        BattleSystem battleSystem;

        private void Awake()
        {
            battleSystem = new BattleSystem();
            battleSystem.ConnectServer();
        }
    }
}
