using Assets.Scripts.Modules.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class SystemBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"{name}  Instance is not null");
            }
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }
    }
}
